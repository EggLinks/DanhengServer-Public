namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// Options used to control the behaviors of <see cref="KcpRawChannelOptions"/>.
    /// </summary>
    public sealed class KcpRawChannelOptions
    {
        /// <summary>
        /// The buffer pool to rent buffer from.
        /// </summary>
        public IKcpBufferPool? BufferPool { get; set; }

        /// <summary>
        /// The maximum packet size that can be transmitted over the underlying transport.
        /// </summary>
        public int Mtu { get; set; } = 1400;

        /// <summary>
        /// The number of packets in the receive queue.
        /// </summary>
        public int ReceiveQueueSize { get; set; } = 32;

        /// <summary>
        /// The number of bytes to reserve at the start of buffer passed into the underlying transport. The transport should fill this reserved space.
        /// </summary>
        public int PreBufferSize { get; set; }

        /// <summary>
        /// The number of bytes to reserve at the end of buffer passed into the underlying transport. The transport should fill this reserved space.
        /// </summary>
        public int PostBufferSize { get; set; }
    }
}
