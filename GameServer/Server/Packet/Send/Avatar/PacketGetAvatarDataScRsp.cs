using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketGetAvatarDataScRsp : BasePacket
    {
        public PacketGetAvatarDataScRsp(PlayerInstance player) : base(CmdIds.GetAvatarDataScRsp)
        {
            var proto = new GetAvatarDataScRsp()
            {
                IsGetAll = true,
            };

            player.AvatarManager?.AvatarData?.Avatars?.ForEach(avatar =>
            {
                if (avatar.GetBaseAvatarId() != 8001)
                    proto.AvatarList.Add(avatar.ToProto());
            });
            proto.AvatarList.Add(player.AvatarManager!.GetHero()!.ToProto());

            SetData(proto);
        }
    }
}
