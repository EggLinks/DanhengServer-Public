using System.Net;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// A transport to send and receive packets.
    /// </summary>
    public interface IKcpTransport
    {
        /// <summary>
        /// Send a packet into the transport.
        /// </summary>
        /// <param name="packet">The content of the packet.</param>
        /// <param name="remoteEndpoint">The remote endpoint</param>
        /// <param name="cancellationToken">A token to cancel this operation.</param>
        /// <returns>A <see cref="ValueTask"/> that completes when the packet is sent.</returns>
        ValueTask SendPacketAsync(Memory<byte> packet, IPEndPoint remoteEndpoint, CancellationToken cancellationToken);
    }
}
