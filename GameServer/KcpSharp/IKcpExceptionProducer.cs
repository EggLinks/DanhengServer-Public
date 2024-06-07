namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// An instance that can produce exceptions in background jobs.
    /// </summary>
    /// <typeparam name="T">The type of the instance.</typeparam>
    public interface IKcpExceptionProducer<out T>
    {
        /// <summary>
        /// Set the handler to invoke when exception is thrown. Return true in the handler to ignore the error and continue running. Return false in the handler to abort the operation.
        /// </summary>
        /// <param name="handler">The exception handler.</param>
        /// <param name="state">The state object to pass into the exception handler.</param>
        void SetExceptionHandler(Func<Exception, T, object?, bool> handler, object? state);
    }
}
