using System.Threading.Tasks.Sources;
using System.Diagnostics;






#if NEED_LINKEDLIST_SHIM
using LinkedListOfQueueItem = KcpSharp.NetstandardShim.LinkedList<(KcpSharp.KcpBuffer Data, byte Fragment)>;
using LinkedListNodeOfQueueItem = KcpSharp.NetstandardShim.LinkedListNode<(KcpSharp.KcpBuffer Data, byte Fragment)>;
#else
using LinkedListOfQueueItem = System.Collections.Generic.LinkedList<(EggLink.DanhengServer.KcpSharp.KcpBuffer Data, byte Fragment)>;
using LinkedListNodeOfQueueItem = System.Collections.Generic.LinkedListNode<(EggLink.DanhengServer.KcpSharp.KcpBuffer Data, byte Fragment)>;
#endif

namespace EggLink.DanhengServer.KcpSharp
{
    internal sealed class KcpReceiveQueue : IValueTaskSource<KcpConversationReceiveResult>, IValueTaskSource<int>, IValueTaskSource<bool>, IDisposable
    {
        private ManualResetValueTaskSourceCore<KcpConversationReceiveResult> _mrvtsc;

        private readonly LinkedListOfQueueItem _queue;
        private readonly bool _stream;
        private readonly int _queueSize;
        private readonly KcpSendReceiveQueueItemCache _cache;
        private int _completedPacketsCount;

        private bool _transportClosed;
        private bool _disposed;

        private bool _activeWait;
        private bool _signaled;
        private byte _operationMode; // 0-receive 1-wait for message 2-wait for available data
        private Memory<byte> _buffer;
        private int _minimumBytes;
        private int _minimumSegments;
        private CancellationToken _cancellationToken;
        private CancellationTokenRegistration _cancellationRegistration;

        public KcpReceiveQueue(bool stream, int queueSize, KcpSendReceiveQueueItemCache cache)
        {
            _mrvtsc = new ManualResetValueTaskSourceCore<KcpConversationReceiveResult>()
            {
                RunContinuationsAsynchronously = true
            };
            _queue = new LinkedListOfQueueItem();
            _stream = stream;
            _queueSize = queueSize;
            _cache = cache;
        }

        public ValueTaskSourceStatus GetStatus(short token) => _mrvtsc.GetStatus(token);
        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
            => _mrvtsc.OnCompleted(continuation, state, token, flags);

        KcpConversationReceiveResult IValueTaskSource<KcpConversationReceiveResult>.GetResult(short token)
        {
            _cancellationRegistration.Dispose();
            try
            {
                return _mrvtsc.GetResult(token);
            }
            finally
            {
                _mrvtsc.Reset();
                lock (_queue)
                {
                    _activeWait = false;
                    _signaled = false;
                    _cancellationRegistration = default;
                }
            }
        }

        int IValueTaskSource<int>.GetResult(short token)
        {
            _cancellationRegistration.Dispose();
            try
            {
                return _mrvtsc.GetResult(token).BytesReceived;
            }
            finally
            {
                _mrvtsc.Reset();
                lock (_queue)
                {
                    _activeWait = false;
                    _signaled = false;
                    _cancellationRegistration = default;
                }
            }
        }

        bool IValueTaskSource<bool>.GetResult(short token)
        {
            _cancellationRegistration.Dispose();
            try
            {
                return !_mrvtsc.GetResult(token).TransportClosed;
            }
            finally
            {
                _mrvtsc.Reset();
                lock (_queue)
                {
                    _activeWait = false;
                    _signaled = false;
                    _cancellationRegistration = default;
                }
            }
        }

        public bool TryPeek(out KcpConversationReceiveResult result)
        {
            lock (_queue)
            {
                if (_disposed || _transportClosed)
                {
                    result = default;
                    return false;
                }
                if (_activeWait)
                {
                    ThrowHelper.ThrowConcurrentReceiveException();
                }

                if (_completedPacketsCount == 0)
                {
                    result = new KcpConversationReceiveResult(0);
                    return false;
                }

                LinkedListNodeOfQueueItem? node = _queue.First;
                if (node is null)
                {
                    result = new KcpConversationReceiveResult(0);
                    return false;
                }

                if (CalculatePacketSize(node, out int packetSize))
                {
                    result = new KcpConversationReceiveResult(packetSize);
                    return true;
                }

                result = default;
                return false;
            }
        }

        public ValueTask<KcpConversationReceiveResult> WaitToReceiveAsync(CancellationToken cancellationToken)
        {
            short token;
            lock (_queue)
            {
                if (_transportClosed || _disposed)
                {
                    return default;
                }
                if (_activeWait)
                {
                    return new ValueTask<KcpConversationReceiveResult>(Task.FromException<KcpConversationReceiveResult>(ThrowHelper.NewConcurrentReceiveException()));
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ValueTask<KcpConversationReceiveResult>(Task.FromCanceled<KcpConversationReceiveResult>(cancellationToken));
                }

                _operationMode = 1;
                _buffer = default;
                _minimumBytes = 0;
                _minimumSegments = 0;

                token = _mrvtsc.Version;
                if (_completedPacketsCount > 0)
                {
                    ConsumePacket(_buffer.Span, out KcpConversationReceiveResult result, out bool bufferTooSmall);
                    ClearPreviousOperation(false);
                    if (bufferTooSmall)
                    {
                        Debug.Assert(false, "This should never be reached.");
                        return new ValueTask<KcpConversationReceiveResult>(Task.FromException<KcpConversationReceiveResult>(ThrowHelper.NewBufferTooSmallForBufferArgument()));
                    }
                    else
                    {
                        return new ValueTask<KcpConversationReceiveResult>(result);
                    }
                }

                _activeWait = true;
                Debug.Assert(!_signaled);
                _cancellationToken = cancellationToken;
            }
            _cancellationRegistration = cancellationToken.UnsafeRegister(state => ((KcpReceiveQueue?)state)!.SetCanceled(), this);

            return new ValueTask<KcpConversationReceiveResult>(this, token);
        }

        public ValueTask<bool> WaitForAvailableDataAsync(int minimumBytes, int minimumSegments, CancellationToken cancellationToken)
        {
            if (minimumBytes < 0)
            {
                return new ValueTask<bool>(Task.FromException<bool>(ThrowHelper.NewArgumentOutOfRangeException(nameof(minimumBytes))));
            }
            if (minimumSegments < 0)
            {
                return new ValueTask<bool>(Task.FromException<bool>(ThrowHelper.NewArgumentOutOfRangeException(nameof(minimumSegments))));
            }

            short token;
            lock (_queue)
            {
                if (_transportClosed || _disposed)
                {
                    return default;
                }
                if (_activeWait)
                {
                    return new ValueTask<bool>(Task.FromException<bool>(ThrowHelper.NewConcurrentReceiveException()));
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ValueTask<bool>(Task.FromCanceled<bool>(cancellationToken));
                }

                if (CheckQueeuSize(_queue, minimumBytes, minimumSegments, _stream))
                {
                    return new ValueTask<bool>(true);
                }

                _activeWait = true;
                Debug.Assert(!_signaled);
                _operationMode = 2;
                _buffer = default;
                _minimumBytes = minimumBytes;
                _minimumSegments = minimumSegments;
                _cancellationToken = cancellationToken;

                token = _mrvtsc.Version;
            }
            _cancellationRegistration = cancellationToken.UnsafeRegister(state => ((KcpReceiveQueue?)state)!.SetCanceled(), this);

            return new ValueTask<bool>(this, token);
        }

        public bool TryReceive(Span<byte> buffer, out KcpConversationReceiveResult result)
        {
            lock (_queue)
            {
                if (_disposed || _transportClosed)
                {
                    result = default;
                    return false;
                }
                if (_activeWait)
                {
                    ThrowHelper.ThrowConcurrentReceiveException();
                }

                if (_completedPacketsCount == 0)
                {
                    result = new KcpConversationReceiveResult(0);
                    return false;
                }

                Debug.Assert(!_signaled);
                _operationMode = 0;

                ConsumePacket(buffer, out result, out bool bufferTooSmall);
                ClearPreviousOperation(false);
                if (bufferTooSmall)
                {
                    ThrowHelper.ThrowBufferTooSmall();
                }
                return true;
            }
        }

        public ValueTask<KcpConversationReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            short token;
            lock (_queue)
            {
                if (_transportClosed || _disposed)
                {
                    return default;
                }
                if (_activeWait)
                {
                    return new ValueTask<KcpConversationReceiveResult>(Task.FromException<KcpConversationReceiveResult>(ThrowHelper.NewConcurrentReceiveException()));
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ValueTask<KcpConversationReceiveResult>(Task.FromCanceled<KcpConversationReceiveResult>(cancellationToken));
                }

                _operationMode = 0;
                _buffer = buffer;

                token = _mrvtsc.Version;
                if (_completedPacketsCount > 0)
                {
                    ConsumePacket(_buffer.Span, out KcpConversationReceiveResult result, out bool bufferTooSmall);
                    ClearPreviousOperation(false);
                    if (bufferTooSmall)
                    {
                        return new ValueTask<KcpConversationReceiveResult>(Task.FromException<KcpConversationReceiveResult>(ThrowHelper.NewBufferTooSmallForBufferArgument()));
                    }
                    else
                    {
                        return new ValueTask<KcpConversationReceiveResult>(result);
                    }
                }

                _activeWait = true;
                Debug.Assert(!_signaled);
                _cancellationToken = cancellationToken;
            }
            _cancellationRegistration = cancellationToken.UnsafeRegister(state => ((KcpReceiveQueue?)state)!.SetCanceled(), this);

            return new ValueTask<KcpConversationReceiveResult>(this, token);
        }

        public ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            short token;
            lock (_queue)
            {
                if (_transportClosed || _disposed)
                {
                    return new ValueTask<int>(Task.FromException<int>(ThrowHelper.NewTransportClosedForStreamException()));
                }
                if (_activeWait)
                {
                    return new ValueTask<int>(Task.FromException<int>(ThrowHelper.NewConcurrentReceiveException()));
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
                }

                _operationMode = 0;
                _buffer = buffer;

                token = _mrvtsc.Version;
                if (_completedPacketsCount > 0)
                {
                    ConsumePacket(_buffer.Span, out KcpConversationReceiveResult result, out bool bufferTooSmall);
                    ClearPreviousOperation(false);
                    if (bufferTooSmall)
                    {
                        return new ValueTask<int>(Task.FromException<int>(ThrowHelper.NewBufferTooSmallForBufferArgument()));
                    }
                    else
                    {
                        return new ValueTask<int>(result.BytesReceived);
                    }
                }

                _activeWait = true;
                Debug.Assert(!_signaled);
                _cancellationToken = cancellationToken;
            }
            _cancellationRegistration = cancellationToken.UnsafeRegister(state => ((KcpReceiveQueue?)state)!.SetCanceled(), this);

            return new ValueTask<int>(this, token);
        }

        public bool CancelPendingOperation(Exception? innerException, CancellationToken cancellationToken)
        {
            lock (_queue)
            {
                if (_activeWait && !_signaled)
                {
                    ClearPreviousOperation(true);
                    _mrvtsc.SetException(ThrowHelper.NewOperationCanceledExceptionForCancelPendingReceive(innerException, cancellationToken));
                    return true;
                }
            }
            return false;
        }

        private void SetCanceled()
        {
            lock (_queue)
            {
                if (_activeWait && !_signaled)
                {
                    CancellationToken cancellationToken = _cancellationToken;
                    ClearPreviousOperation(true);
                    _mrvtsc.SetException(new OperationCanceledException(cancellationToken));
                }
            }
        }

        private void ClearPreviousOperation(bool signaled)
        {
            _signaled = signaled;
            _operationMode = 0;
            _buffer = default;
            _minimumBytes = default;
            _minimumSegments = default;
            _cancellationToken = default;
        }

        public void Enqueue(KcpBuffer buffer, byte fragment)
        {
            lock (_queue)
            {
                if (_transportClosed || _disposed)
                {
                    return;
                }

                if (_stream)
                {
                    if (buffer.Length == 0)
                    {
                        return;
                    }
                    fragment = 0;
                    _queue.AddLast(_cache.Rent(buffer, 0));
                }
                else
                {
                    LinkedListNodeOfQueueItem? lastNode = _queue.Last;
                    if (lastNode is null || lastNode.ValueRef.Fragment == 0 || lastNode.ValueRef.Fragment - 1 == fragment)
                    {
                        _queue.AddLast(_cache.Rent(buffer, fragment));
                    }
                    else
                    {
                        fragment = 0;
                        _queue.AddLast(_cache.Rent(buffer, 0));
                    }
                }

                if (fragment == 0)
                {
                    _completedPacketsCount++;
                    if (_activeWait && !_signaled)
                    {
                        TryCompleteReceive();
                        TryCompleteWaitForData();
                    }
                }
            }
        }

        private void TryCompleteReceive()
        {
            Debug.Assert(_activeWait && !_signaled);

            if (_operationMode <= 1)
            {
                Debug.Assert(_operationMode == 0 || _operationMode == 1);
                ConsumePacket(_buffer.Span, out KcpConversationReceiveResult result, out bool bufferTooSmall);
                ClearPreviousOperation(true);
                if (bufferTooSmall)
                {
                    _mrvtsc.SetException(ThrowHelper.NewBufferTooSmallForBufferArgument());
                }
                else
                {
                    _mrvtsc.SetResult(result);
                }
            }
        }

        private void TryCompleteWaitForData()
        {
            if (_operationMode == 2)
            {
                if (CheckQueeuSize(_queue, _minimumBytes, _minimumSegments, _stream))
                {
                    ClearPreviousOperation(true);
                    _mrvtsc.SetResult(new KcpConversationReceiveResult(0));
                }
            }
        }

        private void ConsumePacket(Span<byte> buffer, out KcpConversationReceiveResult result, out bool bufferTooSmall)
        {
            LinkedListNodeOfQueueItem? node = _queue.First;
            if (node is null)
            {
                result = default;
                bufferTooSmall = false;
                return;
            }

            // peek
            if (_operationMode == 1)
            {
                if (CalculatePacketSize(node, out int bytesRecevied))
                {
                    result = new KcpConversationReceiveResult(bytesRecevied);
                }
                else
                {
                    result = default;
                }
                bufferTooSmall = false;
                return;
            }

            Debug.Assert(_operationMode == 0);

            // ensure buffer is big enough
            int bytesInPacket = 0;
            if (!_stream)
            {
                while (node is not null)
                {
                    bytesInPacket += node.ValueRef.Data.Length;
                    if (node.ValueRef.Fragment == 0)
                    {
                        break;
                    }
                    node = node.Next;
                }

                if (node is null)
                {
                    // incomplete packet
                    result = default;
                    bufferTooSmall = false;
                    return;
                }

                if (bytesInPacket > buffer.Length)
                {
                    result = default;
                    bufferTooSmall = true;
                    return;
                }
            }

            bool anyDataReceived = false;
            bytesInPacket = 0;
            node = _queue.First;
            LinkedListNodeOfQueueItem? next;
            while (node is not null)
            {
                next = node.Next;

                byte fragment = node.ValueRef.Fragment;
                ref KcpBuffer data = ref node.ValueRef.Data;

                int sizeToCopy = Math.Min(data.Length, buffer.Length);
                data.DataRegion.Span.Slice(0, sizeToCopy).CopyTo(buffer);
                buffer = buffer.Slice(sizeToCopy);
                bytesInPacket += sizeToCopy;
                anyDataReceived = true;

                if (sizeToCopy != data.Length)
                {
                    // partial data is received.
                    node.ValueRef = (data.Consume(sizeToCopy), node.ValueRef.Fragment);
                }
                else
                {
                    // full fragment is consumed
                    data.Release();
                    _queue.Remove(node);
                    _cache.Return(node);
                    if (fragment == 0)
                    {
                        _completedPacketsCount--;
                    }
                }

                if (!_stream && fragment == 0)
                {
                    break;
                }

                if (sizeToCopy == 0)
                {
                    break;
                }

                node = next;
            }

            if (!anyDataReceived)
            {
                result = default;
                bufferTooSmall = false;
            }
            else
            {
                result = new KcpConversationReceiveResult(bytesInPacket);
                bufferTooSmall = false;
            }
        }

        private static bool CalculatePacketSize(LinkedListNodeOfQueueItem first, out int packetSize)
        {
            int bytesRecevied = first.ValueRef.Data.Length;
            if (first.ValueRef.Fragment == 0)
            {
                packetSize = bytesRecevied;
                return true;
            }

            LinkedListNodeOfQueueItem? node = first.Next;
            while (node is not null)
            {
                bytesRecevied += node.ValueRef.Data.Length;
                if (node.ValueRef.Fragment == 0)
                {
                    packetSize = bytesRecevied;
                    return true;
                }
                node = node.Next;
            }

            // deadlink
            packetSize = 0;
            return false;
        }

        private static bool CheckQueeuSize(LinkedListOfQueueItem queue, int minimumBytes, int minimumSegments, bool stream)
        {
            LinkedListNodeOfQueueItem? node = queue.First;
            while (node is not null)
            {
                ref KcpBuffer buffer = ref node.ValueRef.Data;
                minimumBytes = Math.Max(minimumBytes - buffer.Length, 0);
                if (stream || node.ValueRef.Fragment == 0)
                {
                    minimumSegments = Math.Max(minimumSegments - 1, 0);
                }
                if (minimumBytes == 0 && minimumSegments == 0)
                {
                    return true;
                }
                node = node.Next;
            }

            return minimumBytes == 0 && minimumSegments == 0;
        }

        public void SetTransportClosed()
        {
            lock (_queue)
            {
                if (_transportClosed || _disposed)
                {
                    return;
                }
                if (_activeWait && !_signaled)
                {
                    ClearPreviousOperation(true);
                    _mrvtsc.SetResult(default);
                }
                _transportClosed = true;
            }
        }

        public int GetQueueSize()
        {
            int count;
            lock (_queue)
            {
                count = _queue.Count;
            }
            return Math.Max(_queue.Count - _queueSize, 0);
        }

        public void Dispose()
        {
            lock (_queue)
            {
                if (_disposed)
                {
                    return;
                }
                if (_activeWait && !_signaled)
                {
                    ClearPreviousOperation(true);
                    _mrvtsc.SetResult(default);
                }
                LinkedListNodeOfQueueItem? node = _queue.First;
                while (node is not null)
                {
                    node.ValueRef.Data.Release();
                    node = node.Next;
                }
                _queue.Clear();
                _disposed = true;
                _transportClosed = true;
            }
        }
    }
}
