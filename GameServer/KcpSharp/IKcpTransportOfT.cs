namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// A transport instance for upper-level connections.
    /// </summary>
    /// <typeparam name="T">The type of the upper-level connection.</typeparam>
    public interface IKcpTransport<out T> : IKcpTransport, IKcpExceptionProducer<IKcpTransport<T>>, IDisposable
    {
        /// <summary>
        /// Get the upper-level connection instace. If Start is not called or the transport is closed, <see cref="InvalidOperationException"/> will be thrown.
        /// </summary>
        /// <exception cref="InvalidOperationException">Start is not called or the transport is closed.</exception>
        T Connection { get; }

        /// <summary>
        /// Create the upper-level connection and start pumping packets from the socket to the upper-level connection.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The current instance is disposed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Start"/> has been called before.</exception>
        void Start();
    }
}
