using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Lineup
{
    public class PacketGetLineupAvatarDataScRsp : BasePacket
    {
        public PacketGetLineupAvatarDataScRsp(PlayerInstance player) : base(CmdIds.GetLineupAvatarDataScRsp)
        {
            var rsp = new GetLineupAvatarDataScRsp();

            player.AvatarManager?.AvatarData?.Avatars?.ForEach(avatar =>
            {
                var data = new LineupAvatarData()
                {
                    Id = (uint)avatar.AvatarId,
                    Hp = (uint)avatar.CurrentHp,
                    AvatarType = AvatarType.AvatarFormalType
                };
                rsp.AvatarDataList.Add(data);
            });

            SetData(rsp);
        }
    }
}
