using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Chat;

public class PacketGetPrivateChatHistoryScRsp : BasePacket
{
    public PacketGetPrivateChatHistoryScRsp(uint contactId, PlayerInstance player) : base(
        CmdIds.GetPrivateChatHistoryScRsp)
    {
        var proto = new GetPrivateChatHistoryScRsp
        {
            ContactId = contactId
        };

        var infos = player.FriendManager!.GetHistoryInfo((int)contactId);
        proto.ChatMessageList.AddRange(infos);

        SetData(proto);
    }
}