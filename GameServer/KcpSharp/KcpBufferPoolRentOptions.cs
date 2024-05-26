namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// The options to use when renting buffers from the pool.
    /// </summary>
    public readonly struct KcpBufferPoolRentOptions : IEquatable<KcpBufferPoolRentOptions>
    {
        private readonly int _size;
        private readonly bool _isOutbound;

        /// <summary>
        /// The minimum size of the buffer.
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// True if the buffer may be passed to the outside of KcpSharp. False if the buffer is only used internally in KcpSharp.
        /// </summary>
        public bool IsOutbound => _isOutbound;

        /// <summary>
        /// Create a <see cref="KcpBufferPoolRentOptions"/> with the specified parameters.
        /// </summary>
        /// <param name="size">The minimum size of the buffer.</param>
        /// <param name="isOutbound">True if the buffer may be passed to the outside of KcpSharp. False if the buffer is only used internally in KcpSharp.</param>
        public KcpBufferPoolRentOptions(int size, bool isOutbound)
        {
            _size = size;
            _isOutbound = isOutbound;
        }

        /// <inheritdoc />
        public bool Equals(KcpBufferPoolRentOptions other) => _size == other._size && _isOutbound == other.IsOutbound;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is KcpBufferPoolRentOptions other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(_size, _isOutbound);
    }
}
