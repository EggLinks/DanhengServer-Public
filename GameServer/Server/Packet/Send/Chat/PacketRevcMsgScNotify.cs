using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Chat;

public class PacketRevcMsgScNotify : BasePacket
{
    public PacketRevcMsgScNotify(uint toUid, uint fromUid, string msg) : base(CmdIds.RevcMsgScNotify)
    {
        var proto = new RevcMsgScNotify
        {
            ChatType = ChatType.Private,
            SourceUid = fromUid,
            TargetUid = toUid,
            MessageText = msg,
            MessageType = MsgType.CustomText
        };

        SetData(proto);
    }

    public PacketRevcMsgScNotify(uint toUid, uint fromUid, uint extraId) : base(CmdIds.RevcMsgScNotify)
    {
        var proto = new RevcMsgScNotify
        {
            ChatType = ChatType.Private,
            SourceUid = fromUid,
            TargetUid = toUid,
            ExtraId = extraId,
            MessageType = MsgType.Emoji
        };

        SetData(proto);
    }
}