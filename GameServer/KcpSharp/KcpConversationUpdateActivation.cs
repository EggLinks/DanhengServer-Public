using System.Diagnostics;
using System.Threading.Tasks.Sources;

namespace EggLink.DanhengServer.KcpSharp
{
    internal sealed class KcpConversationUpdateActivation : IValueTaskSource<KcpConversationUpdateNotification>, IDisposable
    {
        private readonly Timer _timer;
        private ManualResetValueTaskSourceCore<KcpConversationUpdateNotification> _mrvtsc;

        private bool _disposed;
        private bool _notificationPending;
        private bool _signaled;
        private bool _activeWait;
        private CancellationToken _cancellationToken;
        private CancellationTokenRegistration _cancellationRegistration;

        private readonly WaitList _waitList;

        ValueTaskSourceStatus IValueTaskSource<KcpConversationUpdateNotification>.GetStatus(short token) => _mrvtsc.GetStatus(token);
        void IValueTaskSource<KcpConversationUpdateNotification>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _mrvtsc.OnCompleted(continuation, state, token, flags);
        KcpConversationUpdateNotification IValueTaskSource<KcpConversationUpdateNotification>.GetResult(short token)
        {
            _cancellationRegistration.Dispose();

            try
            {
                return _mrvtsc.GetResult(token);
            }
            finally
            {
                _mrvtsc.Reset();

                lock (this)
                {
                    _signaled = false;
                    _activeWait = false;
                    _cancellationRegistration = default;
                }
            }
        }

        public KcpConversationUpdateActivation(int interval)
        {
            _timer = new Timer(state =>
            {
                WeakReference<KcpConversationUpdateActivation>? reference = (WeakReference<KcpConversationUpdateActivation>?)state!;
                if (reference.TryGetTarget(out KcpConversationUpdateActivation? target))
                {
                    target.Notify();
                }
            }, new WeakReference<KcpConversationUpdateActivation>(this), interval, interval);
            _mrvtsc = new ManualResetValueTaskSourceCore<KcpConversationUpdateNotification> { RunContinuationsAsynchronously = true };
            _waitList = new WaitList(this);
        }

        public void Notify()
        {
            if (_disposed)
            {
                return;
            }
            lock (this)
            {
                if (_disposed || _notificationPending)
                {
                    return;
                }
                if (_activeWait && !_signaled)
                {
                    _signaled = true;
                    _cancellationToken = default;
                    _mrvtsc.SetResult(default);
                }
                else
                {
                    _notificationPending = true;
                }
            }
        }

        private void NotifyPacketReceived()
        {
            lock (this)
            {
                if (_disposed)
                {
                    return;
                }
                if (_activeWait && !_signaled)
                {
                    if (_waitList.Occupy(out KcpConversationUpdateNotification notification))
                    {
                        _signaled = true;
                        _cancellationToken = default;
                        bool timerNotification = _notificationPending;
                        _notificationPending = false;
                        _mrvtsc.SetResult(notification.WithTimerNotification(timerNotification));
                    }
                }
            }
        }

        public ValueTask<KcpConversationUpdateNotification> WaitAsync(CancellationToken cancellationToken)
        {
            short token;
            lock (this)
            {
                if (_disposed)
                {
                    return default;
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ValueTask<KcpConversationUpdateNotification>(Task.FromCanceled<KcpConversationUpdateNotification>(cancellationToken));
                }
                if (_activeWait)
                {
                    throw new InvalidOperationException();
                }
                if (_waitList.Occupy(out KcpConversationUpdateNotification notification))
                {
                    bool timerNotification = _notificationPending;
                    _notificationPending = false;
                    return new ValueTask<KcpConversationUpdateNotification>(notification.WithTimerNotification(timerNotification));
                }
                if (_notificationPending)
                {
                    _notificationPending = false;
                    return default;
                }

                _activeWait = true;
                Debug.Assert(!_signaled);
                _cancellationToken = cancellationToken;
                token = _mrvtsc.Version;
            }

            _cancellationRegistration = cancellationToken.UnsafeRegister(state => ((KcpConversationUpdateActivation?)state)!.CancelWaiting(), this);
            return new ValueTask<KcpConversationUpdateNotification>(this, token);
        }

        private void CancelWaiting()
        {
            lock (this)
            {
                if (_activeWait && !_signaled)
                {
                    CancellationToken cancellationToken = _cancellationToken;
                    _signaled = true;
                    _cancellationToken = default;
                    _mrvtsc.SetException(new OperationCanceledException(cancellationToken));
                }
            }
        }

        public ValueTask InputPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
        {
            if (_disposed)
            {
                return default;
            }
            return _waitList.InputPacketAsync(packet, cancellationToken);
        }

        public void Dispose()
        {
            lock (this)
            {
                if (_disposed)
                {
                    return;
                }
                _disposed = true;
                if (_activeWait && !_signaled)
                {
                    _signaled = true;
                    _cancellationToken = default;
                    _mrvtsc.SetResult(default);
                }
            }
            _timer.Dispose();
            _waitList.Dispose();
        }

        private class WaitList : IValueTaskSource, IKcpConversationUpdateNotificationSource, IDisposable
        {
            private readonly KcpConversationUpdateActivation _parent;
            private LinkedList<WaitItem>? _list;
            private ManualResetValueTaskSourceCore<bool> _mrvtsc;

            private bool _available; // activeWait
            private bool _occupied;
            private bool _signaled;
            private bool _disposed;

            private ReadOnlyMemory<byte> _packet;
            private CancellationToken _cancellationToken;
            private CancellationTokenRegistration _cancellationRegistration;

            public ReadOnlyMemory<byte> Packet
            {
                get
                {
                    lock (this)
                    {
                        if (_available && _occupied && !_signaled)
                        {
                            return _packet;
                        }
                    }
                    return default;
                }
            }

            ValueTaskSourceStatus IValueTaskSource.GetStatus(short token) => _mrvtsc.GetStatus(token);
            void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _mrvtsc.OnCompleted(continuation, state, token, flags);
            void IValueTaskSource.GetResult(short token)
            {
                _cancellationRegistration.Dispose();

                try
                {
                    _mrvtsc.GetResult(token);
                }
                finally
                {
                    _mrvtsc.Reset();

                    lock (this)
                    {
                        _available = false;
                        _occupied = false;
                        _signaled = false;
                        _cancellationRegistration = default;
                    }
                }
            }

            public WaitList(KcpConversationUpdateActivation parent)
            {
                _parent = parent;
                _mrvtsc = new ManualResetValueTaskSourceCore<bool> { RunContinuationsAsynchronously = true };
            }

            public ValueTask InputPacketAsync(ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
            {
                WaitItem? waitItem = null;
                short token = 0;
                lock (this)
                {
                    if (_disposed)
                    {
                        return default;
                    }
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new ValueTask(Task.FromCanceled(cancellationToken));
                    }
                    if (_available)
                    {
                        waitItem = new WaitItem(this, packet, cancellationToken);
                        _list ??= new LinkedList<WaitItem>();
                        _list.AddLast(waitItem.Node);
                    }
                    else
                    {
                        token = _mrvtsc.Version;

                        _available = true;
                        Debug.Assert(!_occupied);
                        Debug.Assert(!_signaled);
                        _packet = packet;
                        _cancellationToken = cancellationToken;
                    }
                }

                ValueTask task;

                if (waitItem is null)
                {
                    _cancellationRegistration = cancellationToken.UnsafeRegister(state => ((WaitList?)state)!.CancelWaiting(), this);
                    task = new ValueTask(this, token);
                }
                else
                {
                    waitItem.RegisterCancellationToken();
                    task = new ValueTask(waitItem.Task);
                }

                _parent.NotifyPacketReceived();

                return task;
            }

            private void CancelWaiting()
            {
                lock (this)
                {
                    if (_available && !_occupied && !_signaled)
                    {
                        _signaled = true;
                        CancellationToken cancellationToken = _cancellationToken;
                        _packet = default;
                        _cancellationToken = default;
                        _mrvtsc.SetException(new OperationCanceledException(cancellationToken));
                    }
                }
            }

            public bool Occupy(out KcpConversationUpdateNotification notification)
            {
                lock (this)
                {
                    if (_disposed)
                    {
                        notification = default;
                        return false;
                    }
                    if (_available && !_occupied && !_signaled)
                    {
                        _occupied = true;
                        notification = new KcpConversationUpdateNotification(this, true);
                        return true;
                    }
                    if (_list is null)
                    {
                        notification = default;
                        return false;
                    }
                    LinkedListNode<WaitItem>? node = _list.First;
                    if (node is not null)
                    {
                        _list.Remove(node);
                        notification = new KcpConversationUpdateNotification(node.Value, true);
                        return true;
                    }
                }
                notification = default;
                return false;
            }

            public void Release()
            {
                lock (this)
                {
                    if (_available && _occupied && !_signaled)
                    {
                        _signaled = true;
                        _packet = default;
                        _cancellationToken = default;
                        _mrvtsc.SetResult(true);
                    }
                }
            }

            internal bool TryRemove(WaitItem item)
            {
                lock (this)
                {
                    LinkedList<WaitItem>? list = _list;
                    if (list is null)
                    {
                        return false;
                    }
                    LinkedListNode<WaitItem> node = item.Node;
                    if (node.Previous is null && node.Next is null)
                    {
                        return false;
                    }
                    list.Remove(node);
                    return true;
                }
            }

            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }
                lock (this)
                {
                    _disposed = true;
                    if (_available && !_occupied && !_signaled)
                    {
                        _signaled = true;
                        _packet = default;
                        _cancellationToken = default;
                        _mrvtsc.SetResult(false);
                    }

                    LinkedList<WaitItem>? list = _list;
                    if (list is not null)
                    {
                        _list = null;

                        LinkedListNode<WaitItem>? node = list.First;
                        LinkedListNode<WaitItem>? next = node?.Next;
                        while (node is not null)
                        {
                            node.Value.Release();

                            list.Remove(node);
                            node = next;
                            next = node?.Next;
                        }
                    }

                }
            }

        }

        private class WaitItem : TaskCompletionSource, IKcpConversationUpdateNotificationSource
        {
            private readonly WaitList _parent;
            private ReadOnlyMemory<byte> _packet;
            private CancellationToken _cancellationToken;
            private CancellationTokenRegistration _cancellationRegistration;
            private bool _released;

            public LinkedListNode<WaitItem> Node { get; }

            public ReadOnlyMemory<byte> Packet
            {
                get
                {
                    lock (this)
                    {
                        if (!_released)
                        {
                            return _packet;
                        }
                    }
                    return default;
                }
            }

            public WaitItem(WaitList parent, ReadOnlyMemory<byte> packet, CancellationToken cancellationToken)
            {
                _parent = parent;
                _packet = packet;
                _cancellationToken = cancellationToken;

                Node = new LinkedListNode<WaitItem>(this);
            }

            public void RegisterCancellationToken()
            {
                _cancellationRegistration = _cancellationToken.UnsafeRegister(state => ((WaitItem?)state)!.CancelWaiting(), this);
            }

            private void CancelWaiting()
            {
                CancellationTokenRegistration cancellationRegistration;
                if (_parent.TryRemove(this))
                {
                    CancellationToken cancellationToken;
                    lock (this)
                    {
                        _released = true;
                        cancellationToken = _cancellationToken;
                        cancellationRegistration = _cancellationRegistration;
                        _packet = default;
                        _cancellationToken = default;
                        _cancellationRegistration = default;
                    }
                    TrySetCanceled(cancellationToken);
                }
                _cancellationRegistration.Dispose();
            }

            public void Release()
            {
                CancellationTokenRegistration cancellationRegistration;
                lock (this)
                {
                    _released = true;
                    cancellationRegistration = _cancellationRegistration;
                    _packet = default;
                    _cancellationToken = default;
                    _cancellationRegistration = default;
                }
                TrySetResult();
                cancellationRegistration.Dispose();
            }
        }
    }
}
