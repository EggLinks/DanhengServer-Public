using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Phone;

public class PacketSelectChatBubbleScRsp : BasePacket
{
    public PacketSelectChatBubbleScRsp(uint bubbleId) : base(CmdIds.SelectChatBubbleScRsp)
    {
        var proto = new SelectChatBubbleScRsp
        {
            CurChatBubble = bubbleId
        };

        SetData(proto);
    }
}