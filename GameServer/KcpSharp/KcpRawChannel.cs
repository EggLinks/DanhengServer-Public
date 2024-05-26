using KcpSharp;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// An unreliable channel with a conversation ID.
    /// </summary>
    public sealed class KcpRawChannel : IKcpConversation, IKcpExceptionProducer<KcpRawChannel>
    {
        private readonly IKcpBufferPool _bufferPool;
        private readonly IKcpTransport _transport;
        private readonly IPEndPoint _remoteEndPoint;
        private readonly ulong? _id;
        private readonly int _mtu;
        private readonly int _preBufferSize;
        private readonly int _postBufferSize;

        private CancellationTokenSource? _sendLoopCts;
        private readonly KcpRawReceiveQueue _receiveQueue;
        private readonly KcpRawSendOperation _sendOperation;
        private readonly AsyncAutoResetEvent<int> _sendNotification;

        private Func<Exception, KcpRawChannel, object?, bool>? _exceptionHandler;
        private object? _exceptionHandlerState;

        /// <summary>
        /// Construct a unreliable channel with a conversation ID.
        /// </summary>
        /// <param name="remoteEndPoint">The remote Endpoint</param>
        /// <param name="transport">The underlying transport.</param>
        /// <param name="options">The options of the <see cref="KcpRawChannel"/>.</param>
        public KcpRawChannel(IPEndPoint remoteEndPoint, IKcpTransport transport, KcpRawChannelOptions? options)
            : this(remoteEndPoint, transport, null, options)
        { }

        /// <summary>
        /// Construct a unreliable channel with a conversation ID.
        /// </summary>
        /// <param name="remoteEndPoint">The remote Endpoint</param>
        /// <param name="transport">The underlying transport.</param>
        /// <param name="conversationId">The conversation ID.</param>
        /// <param name="options">The options of the <see cref="KcpRawChannel"/>.</param>
        public KcpRawChannel(IPEndPoint remoteEndPoint, IKcpTransport transport, long conversationId, KcpRawChannelOptions? options)
            : this(remoteEndPoint, transport, (ulong)conversationId, options)
        { }

        private KcpRawChannel(IPEndPoint remoteEndPoint, IKcpTransport transport, ulong? conversationId, KcpRawChannelOptions? options)
        {
            _bufferPool = options?.BufferPool ?? DefaultArrayPoolBufferAllocator.Default;
            _remoteEndPoint = remoteEndPoint;
            _transport = transport;
            _id = conversationId;

            if (options is null)
            {
                _mtu = KcpConversationOptions.MtuDefaultValue;
            }
            else if (options.Mtu < 50)
            {
                throw new ArgumentException("MTU must be at least 50.", nameof(options));
            }
            else
            {
                _mtu = options.Mtu;
            }

            _preBufferSize = options?.PreBufferSize ?? 0;
            _postBufferSize = options?.PostBufferSize ?? 0;
            if (_preBufferSize < 0)
            {
                throw new ArgumentException("PreBufferSize must be a non-negative integer.", nameof(options));
            }
            if (_postBufferSize < 0)
            {
                throw new ArgumentException("PostBufferSize must be a non-negative integer.", nameof(options));
            }
            if ((uint)(_preBufferSize + _postBufferSize) >= (uint)_mtu)
            {
                throw new ArgumentException("The sum of PreBufferSize and PostBufferSize must be less than MTU.", nameof(options));
            }
            if (conversationId.HasValue && (uint)(_preBufferSize + _postBufferSize) >= (uint)(_mtu - 4))
            {
                throw new ArgumentException("The sum of PreBufferSize and PostBufferSize is too large. There is not enough space in the packet for the conversation ID.", nameof(options));
            }

            int queueSize = options?.ReceiveQueueSize ?? 32;
            if (queueSize < 1)
            {
                throw new ArgumentException("QueueSize must be a positive integer.", nameof(options));
            }

            _sendLoopCts = new CancellationTokenSource();
            _sendNotification = new AsyncAutoResetEvent<int>();
            _receiveQueue = new KcpRawReceiveQueue(_bufferPool, queueSize);
            _sendOperation = new KcpRawSendOperation(_sendNotification);

            RunSendLoop();
        }

        /// <summary>
        /// Set the handler to invoke when exception is thrown during flushing packets to the transport. Return true in the handler to ignore the error and continue running. Return false in the handler to abort the operation and mark the transport as closed.
        /// </summary>
        /// <param name="handler">The exception handler.</param>
        /// <param name="state">The state object to pass into the exception handler.</param>
        public void SetExceptionHandler(Func<Exception, KcpRawChannel, object?, bool> handler, object? state)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _exceptionHandler = handler;
            _exceptionHandlerState = state;
        }

        /// <summary>
        /// Get the ID of the current conversation.
        /// </summary>
        public long? ConversationId => (long?)_id;

        /// <summary>
        /// Get whether the transport is marked as closed.
        /// </summary>
        public bool TransportClosed => _sendLoopCts is null;

        /// <summary>
        /// Send message to the underlying transport.
        /// </summary>
        /// <param name="buffer">The content of the message</param>
        /// <param name="cancellationToken">The token to cancel this operation.</param>
        /// <exception cref="ArgumentException">The size of the message is larger than mtu, thus it can not be sent.</exception>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> is fired before send operation is completed.</exception>
        /// <exception cref="InvalidOperationException">The send operation is initiated concurrently.</exception>
        /// <exception cref="ObjectDisposedException">The <see cref="KcpConversation"/> instance is disposed.</exception>
        /// <returns>A <see cref="ValueTask{Boolean}"/> that completes when the entire message is put into the queue. The result of the task is false when the transport is closed.</returns>
        public ValueTask<bool> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            => _sendOperation.SendAsync(buffer, cancellationToken);


        /// <summary>
        /// Cancel the current send operation or flush operation.
        /// </summary>
        /// <returns>True if the current operation is canceled. False if there is no active send operation.</returns>
        public bool CancelPendingSend()
            => _sendOperation.CancelPendingOperation(null, default);

        /// <summary>
        /// Cancel the current send operation or flush operation.
        /// </summary>
        /// <param name="innerException">The inner exception of the <see cref="OperationCanceledException"/> thrown by the <see cref="SendAsync(ReadOnlyMemory{byte}, CancellationToken)"/> method.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> in the <see cref="OperationCanceledException"/> thrown by the <see cref="SendAsync(ReadOnlyMemory{byte}, CancellationToken)"/> method.</param>
        /// <returns>True if the current operation is canceled. False if there is no active send operation.</returns>
        public bool CancelPendingSend(Exception? innerException, CancellationToken cancellationToken)
            => _sendOperation.CancelPendingOperation(innerException, cancellationToken);


        private async void RunSendLoop()
        {
            CancellationToken cancellationToken = _sendLoopCts?.Token ?? new CancellationToken(true);
            KcpRawSendOperation sendOperation = _sendOperation;
            AsyncAutoResetEvent<int> ev = _sendNotification;
            int mss = _mtu - _preBufferSize - _postBufferSize;
            if (_id.HasValue)
            {
                mss -= 8;
            }

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int payloadSize = await ev.WaitAsync().ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    if (payloadSize < 0 || payloadSize > mss)
                    {
                        _ = sendOperation.TryConsume(default, out _);
                        continue;
                    }

                    int overhead = _preBufferSize + _postBufferSize;
                    if (_id.HasValue)
                    {
                        overhead += 8;
                    }
                    {
                        using KcpRentedBuffer owner = _bufferPool.Rent(new KcpBufferPoolRentOptions(payloadSize + overhead, true));
                        Memory<byte> memory = owner.Memory;

                        // Fill the buffer
                        if (_preBufferSize != 0)
                        {
                            memory.Span.Slice(0, _preBufferSize).Clear();
                            memory = memory.Slice(_preBufferSize);
                        }
                        if (_id.HasValue)
                        {
                            BinaryPrimitives.WriteUInt64LittleEndian(memory.Span, _id.GetValueOrDefault());
                            memory = memory.Slice(8);
                        }
                        if (!sendOperation.TryConsume(memory, out int bytesWritten))
                        {
                            continue;
                        }
                        payloadSize = Math.Min(payloadSize, bytesWritten);
                        memory = memory.Slice(payloadSize);
                        if (_postBufferSize != 0)
                        {
                            memory.Span.Slice(0, _postBufferSize).Clear();
                        }

                        // Send the buffer
                        try
                        {
                            await _transport.SendPacketAsync(owner.Memory.Slice(0, payloadSize + overhead), _remoteEndPoint, cancellationToken).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            if (!HandleFlushException(ex))
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
            catch (Exception ex)
            {
                HandleFlushException(ex);
            }
        }


        private bool HandleFlushException(Exception ex)
        {
            Func<Exception, KcpRawChannel, object?, bool>? handler = _exceptionHandler;
            object? state = _exceptionHandlerState;
            bool result = false;
            if (handler is not null)
            {
                try
                {
                    result = handler.Invoke(ex, this, state);
                }
                catch
                {
                    result = false;
                }
            }

            if (!result)
            {
                SetTransportClosed();
            }
            return result;
        }

        /// <inheritdoc />
        public ValueTask InputPakcetAsync(UdpReceiveResult packet, CancellationToken cancellationToken = default)
        {
            ReadOnlySpan<byte> span = packet.Buffer.AsSpan();
            int overhead = _id.HasValue ? KcpGlobalVars.CONVID_LENGTH : 0;
            if (span.Length < overhead || span.Length > _mtu)
            {
                return default;
            }
            if (_id.HasValue)
            {
                if (BinaryPrimitives.ReadUInt64BigEndian(span) != _id.GetValueOrDefault())
                {
                    return default;
                }
                span = span.Slice(8);
            }
            _receiveQueue.Enqueue(span);
            return default;
        }

        /// <summary>
        /// Get the size of the next available message in the receive queue.
        /// </summary>
        /// <param name="result">The transport state and the size of the next available message.</param>
        /// <exception cref="InvalidOperationException">The receive or peek operation is initiated concurrently.</exception>
        /// <returns>True if the receive queue contains at least one message. False if the receive queue is empty or the transport is closed.</returns>
        public bool TryPeek(out KcpConversationReceiveResult result)
            => _receiveQueue.TryPeek(out result);

        /// <summary>
        /// Remove the next available message in the receive queue and copy its content into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to receive message.</param>
        /// <param name="result">The transport state and the count of bytes moved into <paramref name="buffer"/>.</param>
        /// <exception cref="ArgumentException">The size of the next available message is larger than the size of <paramref name="buffer"/>.</exception>
        /// <exception cref="InvalidOperationException">The receive or peek operation is initiated concurrently.</exception>
        /// <returns>True if the next available message is moved into <paramref name="buffer"/>. False if the receive queue is empty or the transport is closed.</returns>
        public bool TryReceive(Span<byte> buffer, out KcpConversationReceiveResult result)
            => _receiveQueue.TryReceive(buffer, out result);

        /// <summary>
        /// Wait until the receive queue contains at least one message.
        /// </summary>
        /// <param name="cancellationToken">The token to cancel this operation.</param>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> is fired before receive operation is completed.</exception>
        /// <exception cref="InvalidOperationException">The receive or peek operation is initiated concurrently.</exception>
        /// <returns>A <see cref="ValueTask{KcpConversationReceiveResult}"/> that completes when the receive queue contains at least one full message, or at least one byte in stream mode. Its result contains the transport state and the size of the available message.</returns>
        public ValueTask<KcpConversationReceiveResult> WaitToReceiveAsync(CancellationToken cancellationToken)
            => _receiveQueue.WaitToReceiveAsync(cancellationToken);

        /// <summary>
        /// Wait for the next full message to arrive if the receive queue is empty. Remove the next available message in the receive queue and copy its content into <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The buffer to receive message.</param>
        /// <param name="cancellationToken">The token to cancel this operation.</param>
        /// <exception cref="ArgumentException">The size of the next available message is larger than the size of <paramref name="buffer"/>.</exception>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> is fired before send operation is completed.</exception>
        /// <exception cref="InvalidOperationException">The receive or peek operation is initiated concurrently.</exception>
        /// <returns>A <see cref="ValueTask{KcpConversationReceiveResult}"/> that completes when a message is moved into <paramref name="buffer"/> or the transport is closed. Its result contains the transport state and the count of bytes written into <paramref name="buffer"/>.</returns>
        public ValueTask<KcpConversationReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            => _receiveQueue.ReceiveAsync(buffer, cancellationToken);


        /// <summary>
        /// Cancel the current receive operation.
        /// </summary>
        /// <returns>True if the current operation is canceled. False if there is no active send operation.</returns>
        public bool CancelPendingReceive()
            => _receiveQueue.CancelPendingOperation(null, default);

        /// <summary>
        /// Cancel the current send operation or flush operation.
        /// </summary>
        /// <param name="innerException">The inner exception of the <see cref="OperationCanceledException"/> thrown by the <see cref="ReceiveAsync(Memory{byte}, CancellationToken)"/> method or <see cref="WaitToReceiveAsync(CancellationToken)"/> method.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> in the <see cref="OperationCanceledException"/> thrown by the <see cref="ReceiveAsync(Memory{byte}, CancellationToken)"/> method or <see cref="WaitToReceiveAsync(CancellationToken)"/> method.</param>
        /// <returns>True if the current operation is canceled. False if there is no active send operation.</returns>
        public bool CancelPendingReceive(Exception? innerException, CancellationToken cancellationToken)
            => _receiveQueue.CancelPendingOperation(innerException, cancellationToken);


        /// <inheritdoc />
        public void SetTransportClosed()
        {
            CancellationTokenSource? cts = Interlocked.Exchange(ref _sendLoopCts, null);
            if (cts is not null)
            {
                cts.Cancel();
                cts.Dispose();
            }

            _receiveQueue.SetTransportClosed();
            _sendOperation.SetTransportClosed();
            _sendNotification.Set(0);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            SetTransportClosed();
            _receiveQueue.Dispose();
            _sendOperation.Dispose();
        }
    }
}
