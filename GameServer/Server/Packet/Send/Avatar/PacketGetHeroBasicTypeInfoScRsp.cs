using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketGetHeroBasicTypeInfoScRsp : BasePacket
    {
        public PacketGetHeroBasicTypeInfoScRsp(PlayerInstance player) : base(CmdIds.GetHeroBasicTypeInfoScRsp)
        {
            var proto = new GetHeroBasicTypeInfoScRsp()
            {
                Gender = player.Data.CurrentGender,
                CurBasicType = (HeroBasicType)player.Data.CurBasicType,
            };

            if (player.AvatarManager?.GetHero() != null)
                proto.BasicTypeInfoList.AddRange(player.AvatarManager?.GetHero()?.ToHeroProto());

            SetData(proto);
        }
    }
}
