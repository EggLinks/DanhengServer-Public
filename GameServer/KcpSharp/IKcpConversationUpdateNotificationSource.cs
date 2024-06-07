namespace EggLink.DanhengServer.KcpSharp
{
    internal interface IKcpConversationUpdateNotificationSource
    {
        ReadOnlyMemory<byte> Packet { get; }
        void Release();
    }
}
