using KcpSharp;
using System.Net;
using System.Net.Sockets;

namespace EggLink.DanhengServer.KcpSharp
{

    /// <summary>
    /// Helper methods to create socket transports for KCP conversations.
    /// </summary>
    public static class KcpSocketTransport
    {
        /// <summary>
        /// Create a socket transport for KCP covnersation.
        /// </summary>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="endPoint">The remote endpoint.</param>
        /// <param name="conversationId">The conversation ID.</param>
        /// <param name="options">The options of the <see cref="KcpConversation"/>.</param>
        /// <returns>The created socket transport instance.</returns>
        public static IKcpTransport<KcpConversation> CreateConversation(UdpClient listener, IPEndPoint endPoint, long conversationId, KcpConversationOptions? options)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }
            if (endPoint is null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            return new KcpSocketTransportForConversation(listener, endPoint, conversationId, options);
        }

        /// <summary>
        /// Create a socket transport for KCP covnersation with no conversation ID.
        /// </summary>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="endPoint">The remote endpoint.</param>
        /// <param name="options">The options of the <see cref="KcpConversation"/>.</param>
        /// <returns>The created socket transport instance.</returns>
        public static IKcpTransport<KcpConversation> CreateConversation(UdpClient listener, IPEndPoint endPoint, KcpConversationOptions? options)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }
            if (endPoint is null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            return new KcpSocketTransportForConversation(listener, endPoint, null, options);
        }

        /// <summary>
        /// Create a socket transport for raw channel.
        /// </summary>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="endPoint">The remote endpoint.</param>
        /// <param name="conversationId">The conversation ID.</param>
        /// <param name="options">The options of the <see cref="KcpRawChannel"/>.</param>
        /// <returns>The created socket transport instance.</returns>
        public static IKcpTransport<KcpRawChannel> CreateRawChannel(UdpClient listener, IPEndPoint endPoint, long conversationId, KcpRawChannelOptions? options)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }
            if (endPoint is null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            return new KcpSocketTransportForRawChannel(listener, endPoint, conversationId, options);
        }

        /// <summary>
        /// Create a socket transport for raw channel with no conversation ID.
        /// </summary>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="endPoint">The remote endpoint.</param>
        /// <param name="options">The options of the <see cref="KcpRawChannel"/>.</param>
        /// <returns>The created socket transport instance.</returns>
        public static IKcpTransport<KcpRawChannel> CreateRawChannel(UdpClient listener, IPEndPoint endPoint, KcpRawChannelOptions? options)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }
            if (endPoint is null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            return new KcpSocketTransportForRawChannel(listener, endPoint, null, options);
        }

        /// <summary>
        /// Create a socket transport for multiplex connection.
        /// </summary>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="mtu">The maximum packet size that can be transmitted over the socket.</param>
        /// <returns></returns>
        public static IKcpTransport<IKcpMultiplexConnection> CreateMultiplexConnection(UdpClient listener, int mtu)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            return new KcpSocketTransportForMultiplexConnection<object>(listener, mtu);
        }

        /// <summary>
        /// Create a socket transport for multiplex connection.
        /// </summary>
        /// <typeparam name="T">The type of the user state.</typeparam>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="mtu">The maximum packet size that can be transmitted over the socket.</param>
        /// <returns></returns>
        public static IKcpTransport<IKcpMultiplexConnection<T>> CreateMultiplexConnection<T>(UdpClient listener, IPEndPoint endPoint, int mtu)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }
            if (endPoint is null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            return new KcpSocketTransportForMultiplexConnection<T>(listener, mtu);
        }

        /// <summary>
        /// Create a socket transport for multiplex connection.
        /// </summary>
        /// <typeparam name="T">The type of the user state.</typeparam>
        /// <param name="listener">The udp listener instance.</param>
        /// <param name="endPoint">The remote endpoint.</param>
        /// <param name="mtu">The maximum packet size that can be transmitted over the socket.</param>
        /// <param name="disposeAction">The action to invoke when state object is removed.</param>
        /// <returns></returns>
        public static IKcpTransport<IKcpMultiplexConnection<T>> CreateMultiplexConnection<T>(UdpClient listener, EndPoint endPoint, int mtu, Action<T?>? disposeAction)
        {
            if (listener is null)
            {
                throw new ArgumentNullException(nameof(listener));
            }
            if (endPoint is null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            return new KcpSocketTransportForMultiplexConnection<T>(listener, mtu, disposeAction);
        }
    }
}
