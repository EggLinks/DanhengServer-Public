using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Util;
using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// A Socket transport for upper-level connections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class KcpSocketTransport<T> : IKcpTransport, IDisposable where T : class, IKcpConversation
    {
        private readonly UdpClient _udpListener;
        private readonly int _mtu;
        private T? _connection;
        private CancellationTokenSource? _cts;
        private bool _disposed;

        /// <summary>
        /// Construct a socket transport with the specified socket and remote endpoint.
        /// </summary>
        /// <param name="socket">The socket instance.</param>
        /// <param name="mtu">The maximum packet size that can be transmitted.</param>
        protected KcpSocketTransport(UdpClient listener, int mtu)
        {
            _udpListener = listener ?? throw new ArgumentNullException(nameof(listener));
            _mtu = mtu;
            if (mtu < 50)
            {
                throw new ArgumentOutOfRangeException(nameof(mtu));
            }
        }

        /// <summary>
        /// Get the upper-level connection instace. If Start is not called or the transport is closed, <see cref="InvalidOperationException"/> will be thrown.
        /// </summary>
        /// <exception cref="InvalidOperationException">Start is not called or the transport is closed.</exception>
        public T Connection => _connection ?? throw new InvalidOperationException();

        /// <summary>
        /// Create the upper-level connection instance.
        /// </summary>
        /// <returns>The upper-level connection instance.</returns>
        protected abstract T Activate();

        /// <summary>
        /// Allocate a block of memory used to receive from socket.
        /// </summary>
        /// <param name="size">The minimum size of the buffer.</param>
        /// <returns>The allocated memory buffer.</returns>
        protected virtual IMemoryOwner<byte> AllocateBuffer(int size)
        {
#if NEED_POH_SHIM
            return MemoryPool<byte>.Shared.Rent(size);
#else
            return new ArrayMemoryOwner(GC.AllocateUninitializedArray<byte>(size, pinned: true));
#endif
        }

        /// <summary>
        /// Handle exception thrown when receiving from remote endpoint.
        /// </summary>
        /// <param name="ex">The exception thrown.</param>
        /// <returns>Whether error should be ignored.</returns>
        protected virtual bool HandleException(Exception ex) => false;

        /// <summary>
        /// Create the upper-level connection and start pumping packets from the socket to the upper-level connection.
        /// </summary>
        public void Start()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(KcpSocketTransport));
            }
            if (_connection is not null)
            {
                throw new InvalidOperationException();
            }

            _connection = Activate();
            if (_connection is null)
            {
                throw new InvalidOperationException();
            }
            _cts = new CancellationTokenSource();
            RunReceiveLoop();
        }

        /// <inheritdoc />
        public ValueTask SendPacketAsync(Memory<byte> packet, IPEndPoint endpoint, CancellationToken cancellationToken = default)
        {
            if (_disposed)
            {
                return default;
            }
            if (packet.Length > _mtu)
            {
                return default;
            }

            return new ValueTask(_udpListener.SendAsync(packet.ToArray(), endpoint, cancellationToken).AsTask());
        }

        private async void RunReceiveLoop()
        {
            CancellationToken cancellationToken = _cts?.Token ?? new CancellationToken(true);
            IKcpConversation? connection = _connection;
            if (connection is null || cancellationToken.IsCancellationRequested)
            {
                return;
            }

            using IMemoryOwner<byte> memoryOwner = AllocateBuffer(_mtu);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int bytesReceived = 0;
                    bool error = false;
                    UdpReceiveResult result = default;
                    try
                    {
                        result = await _udpListener.ReceiveAsync(cancellationToken);
                        bytesReceived = result.Buffer.Length;
                    }
                    catch
                    {
                    }

                    if (bytesReceived != 0 && bytesReceived <= _mtu)
                    {
                        if (bytesReceived == Listener.HANDSHAKE_SIZE)
                            await Listener.HandleHandshake(result);
                        else if (!error)
                            await connection.InputPakcetAsync(result, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
            catch (Exception ex)
            {
                HandleExceptionWrapper(ex);
            }
        }

        private bool HandleExceptionWrapper(Exception ex)
        {
            bool result;
            try
            {
                new Logger("KcpServer").Error("KCP Error:", ex);
                result = HandleException(ex);
            }
            catch
            {
                result = false;
            }

            _connection?.SetTransportClosed();
            CancellationTokenSource? cts = Interlocked.Exchange(ref _cts, null);
            if (cts is not null)
            {
                cts.Cancel();
                cts.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Dispose all the managed and the unmanaged resources used by this instance.
        /// </summary>
        /// <param name="disposing">If managed resources should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    CancellationTokenSource? cts = Interlocked.Exchange(ref _cts, null);
                    if (cts is not null)
                    {
                        cts.Cancel();
                        cts.Dispose();
                    }
                    _connection?.Dispose();
                }

                _connection = null;
                _cts = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// Dispose the unmanaged resources used by this instance.
        /// </summary>
        ~KcpSocketTransport()
        {
            Dispose(disposing: false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
