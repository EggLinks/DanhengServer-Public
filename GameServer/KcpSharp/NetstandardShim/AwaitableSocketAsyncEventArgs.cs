#if NEED_SOCKET_SHIM

using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace KcpSharp
{
    internal class AwaitableSocketAsyncEventArgs : SocketAsyncEventArgs, IValueTaskSource
    {
        private ManualResetValueTaskSourceCore<bool> _mrvtsc = new ManualResetValueTaskSourceCore<bool> { RunContinuationsAsynchronously = true };

        void IValueTaskSource.GetResult(short token) => _mrvtsc.GetResult(token);
        ValueTaskSourceStatus IValueTaskSource.GetStatus(short token) => _mrvtsc.GetStatus(token);
        void IValueTaskSource.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
            => _mrvtsc.OnCompleted(continuation, state, token, flags);

        protected override void OnCompleted(SocketAsyncEventArgs e)
        {
            _mrvtsc.SetResult(true);
        }

        public ValueTask WaitAsync()
        {
            return new ValueTask(this, _mrvtsc.Version);
        }

        public void Reset()
        {
            _mrvtsc.Reset();
        }
    }
}

#endif
