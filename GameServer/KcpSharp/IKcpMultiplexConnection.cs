using KcpSharp;
using System.Net;

namespace EggLink.DanhengServer.KcpSharp
{
    /// <summary>
    /// Multiplex many channels or conversations over the same transport.
    /// </summary>
    public interface IKcpMultiplexConnection : IDisposable
    {
        /// <summary>
        /// Determine whether the multiplex connection contains a conversation with the specified id.
        /// </summary>
        /// <param name="id">The conversation ID.</param>
        /// <returns>True if the multiplex connection contains the specified conversation. Otherwise false.</returns>
        bool Contains(long id);

        /// <summary>
        /// Create a raw channel with the specified conversation ID.
        /// </summary>
        /// <param name="id">The conversation ID.</param>
        /// <param name="remoteEndpoint">The remote endpoint</param>
        /// <param name="options">The options of the <see cref="KcpRawChannel"/>.</param>
        /// <returns>The raw channel created.</returns>
        /// <exception cref="ObjectDisposedException">The current instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">Another channel or conversation with the same ID was already registered.</exception>
        KcpRawChannel CreateRawChannel(long id, IPEndPoint remoteEndpoint, KcpRawChannelOptions? options = null);

        /// <summary>
        /// Create a conversation with the specified conversation ID.
        /// </summary>
        /// <param name="id">The conversation ID.</param>
        /// <param name="remoteEndpoint">The remote endpoint</param>
        /// <param name="options">The options of the <see cref="KcpConversation"/>.</param>
        /// <returns>The KCP conversation created.</returns>
        /// <exception cref="ObjectDisposedException">The current instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">Another channel or conversation with the same ID was already registered.</exception>
        KcpConversation CreateConversation(long id, IPEndPoint remoteEndpoint, KcpConversationOptions? options = null);

        /// <summary>
        /// Register a conversation or channel with the specified conversation ID and user state.
        /// </summary>
        /// <param name="conversation">The conversation or channel to register.</param>
        /// <param name="id">The conversation ID.</param>
        /// <exception cref="ArgumentNullException"><paramref name="conversation"/> is not provided.</exception>
        /// <exception cref="ObjectDisposedException">The current instance is disposed.</exception>
        /// <exception cref="InvalidOperationException">Another channel or conversation with the same ID was already registered.</exception>
        void RegisterConversation(IKcpConversation conversation, long id);


        /// <summary>
        /// Unregister a conversation or channel with the specified conversation ID.
        /// </summary>
        /// <param name="id">The conversation ID.</param>
        /// <returns>The conversation unregistered. Returns null when the conversation with the specified ID is not found.</returns>
        IKcpConversation? UnregisterConversation(long id);
    }
}
