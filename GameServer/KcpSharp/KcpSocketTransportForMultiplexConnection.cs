using System.Net.Sockets;

namespace EggLink.DanhengServer.KcpSharp
{
    internal sealed class KcpSocketTransportForMultiplexConnection<T> : KcpSocketTransport<KcpMultiplexConnection<T>>, IKcpTransport<IKcpMultiplexConnection<T>>
    {
        private readonly Action<T?>? _disposeAction;
        private Func<Exception, IKcpTransport<IKcpMultiplexConnection<T>>, object?, bool>? _exceptionHandler;
        private object? _exceptionHandlerState;

        internal KcpSocketTransportForMultiplexConnection(UdpClient listener, int mtu)
            : base(listener, mtu)
        { }

        internal KcpSocketTransportForMultiplexConnection(UdpClient listener, int mtu, Action<T?>? disposeAction)
            : base(listener, mtu)
        {
            _disposeAction = disposeAction;
        }

        protected override KcpMultiplexConnection<T> Activate() => new(this, _disposeAction);

        IKcpMultiplexConnection<T> IKcpTransport<IKcpMultiplexConnection<T>>.Connection => Connection;

        protected override bool HandleException(Exception ex)
        {
            if (_exceptionHandler is not null)
            {
                return _exceptionHandler.Invoke(ex, this, _exceptionHandlerState);
            }
            return false;
        }

        public void SetExceptionHandler(Func<Exception, IKcpTransport<IKcpMultiplexConnection<T>>, object?, bool> handler, object? state)
        {
            _exceptionHandler = handler;
            _exceptionHandlerState = state;
        }
    }
}
