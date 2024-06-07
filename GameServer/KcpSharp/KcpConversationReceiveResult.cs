using System.Globalization;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// The result of a receive or peek operation.
    /// </summary>
    public readonly struct KcpConversationReceiveResult : IEquatable<KcpConversationReceiveResult>
    {
        private readonly int _bytesReceived;
        private readonly bool _connectionAlive;

        /// <summary>
        /// The number of bytes received.
        /// </summary>
        public int BytesReceived => _bytesReceived;

        /// <summary>
        /// Whether the underlying transport is marked as closed.
        /// </summary>
        public bool TransportClosed => !_connectionAlive;

        /// <summary>
        /// Construct a <see cref="KcpConversationReceiveResult"/> with the specified number of bytes received.
        /// </summary>
        /// <param name="bytesReceived">The number of bytes received.</param>
        public KcpConversationReceiveResult(int bytesReceived)
        {
            _bytesReceived = bytesReceived;
            _connectionAlive = true;
        }

        /// <summary>
        /// Checks whether the two instance is equal.
        /// </summary>
        /// <param name="left">The one instance.</param>
        /// <param name="right">The other instance.</param>
        /// <returns>Whether the two instance is equal</returns>
        public static bool operator ==(KcpConversationReceiveResult left, KcpConversationReceiveResult right) => left.Equals(right);

        /// <summary>
        /// Checks whether the two instance is not equal.
        /// </summary>
        /// <param name="left">The one instance.</param>
        /// <param name="right">The other instance.</param>
        /// <returns>Whether the two instance is not equal</returns>
        public static bool operator !=(KcpConversationReceiveResult left, KcpConversationReceiveResult right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(KcpConversationReceiveResult other) => BytesReceived == other.BytesReceived && TransportClosed == other.TransportClosed;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is KcpConversationReceiveResult other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(BytesReceived, TransportClosed);

        /// <inheritdoc />
        public override string ToString() => _connectionAlive ? _bytesReceived.ToString(CultureInfo.InvariantCulture) : "Transport is closed.";
    }
}
