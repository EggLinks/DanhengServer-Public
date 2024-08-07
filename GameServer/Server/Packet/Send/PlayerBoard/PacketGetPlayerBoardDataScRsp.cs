using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;

public class PacketGetPlayerBoardDataScRsp : BasePacket
{
    public PacketGetPlayerBoardDataScRsp(PlayerInstance player) : base(CmdIds.GetPlayerBoardDataScRsp)
    {
        var proto = new GetPlayerBoardDataScRsp
        {
            CurrentHeadIconId = (uint)player.Data.HeadIcon,
            Signature = player.Data.Signature
        };

        player.PlayerUnlockData?.HeadIcons.ForEach(id =>
        {
            HeadIconData headIcon = new() { Id = (uint)id };
            proto.UnlockedHeadIconList.Add(headIcon);
        });

        proto.DisplayAvatarVec = new DisplayAvatarVec();
        var pos = 0;
        player.AvatarManager?.AvatarData!.DisplayAvatars.ForEach(avatar =>
        {
            DisplayAvatarData displayAvatar = new()
            {
                AvatarId = (uint)avatar,
                Pos = (uint)pos++
            };
            proto.DisplayAvatarVec.DisplayAvatarList.Add(displayAvatar);
        });
        player.AvatarManager?.AvatarData!.AssistAvatars.ForEach(x => proto.AssistAvatarIdList.Add((uint)x));

        SetData(proto);
    }
}