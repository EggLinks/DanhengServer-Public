using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;

public class PacketGetAvatarDataScRsp : BasePacket
{
    public PacketGetAvatarDataScRsp(PlayerInstance player) : base(CmdIds.GetAvatarDataScRsp)
    {
        var proto = new GetAvatarDataScRsp
        {
            IsGetAll = true
        };

        player.AvatarManager?.AvatarData?.Avatars?.ForEach(avatar =>
        {
            GameData.MultiplePathAvatarConfigData.TryGetValue(avatar.AvatarId, out var multiPathAvatar);

            if (multiPathAvatar == null)
            {
                // Normal avatar
                proto.AvatarList.Add(avatar.ToProto());
            }
            else
            {
                // Multiple path avatar
                if (avatar.AvatarId == multiPathAvatar.BaseAvatarID) proto.AvatarList.Add(avatar.ToProto());
            }
        });

        SetData(proto);
    }
}