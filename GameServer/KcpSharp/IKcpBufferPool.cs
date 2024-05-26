namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// The buffer pool to rent buffers from.
    /// </summary>
    public interface IKcpBufferPool
    {
        /// <summary>
        /// Rent a buffer using the specified options.
        /// </summary>
        /// <param name="options">The options used to rent this buffer.</param>
        /// <returns></returns>
        KcpRentedBuffer Rent(KcpBufferPoolRentOptions options);
    }
}
