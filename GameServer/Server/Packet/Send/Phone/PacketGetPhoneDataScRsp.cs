using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Phone;

public class PacketGetPhoneDataScRsp : BasePacket
{
    public PacketGetPhoneDataScRsp(PlayerInstance player) : base(CmdIds.GetPhoneDataScRsp)
    {
        if (!GameData.ChatBubbleConfigData.ContainsKey(player.Data.ChatBubble)) // to avoid npe
            player.Data.ChatBubble = 220000;

        var proto = new GetPhoneDataScRsp
        {
            CurChatBubble = (uint)player.Data.ChatBubble,
            CurPhoneTheme = (uint)player.Data.PhoneTheme
        };

        foreach (var item in player.PlayerUnlockData!.PhoneThemes) proto.OwnedPhoneThemes.Add((uint)item);

        foreach (var item in player.PlayerUnlockData!.ChatBubbles) proto.OwnedChatBubbles.Add((uint)item);

        SetData(proto);
    }
}