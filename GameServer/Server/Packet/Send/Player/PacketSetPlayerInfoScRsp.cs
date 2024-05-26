using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSetPlayerInfoScRsp : BasePacket
    {
        public PacketSetPlayerInfoScRsp(PlayerInstance player, bool IsModify) : base(CmdIds.SetPlayerInfoScRsp)
        {
            var proto = new SetPlayerInfoScRsp()
            {
                CurBasicType = (HeroBasicType)player.Data.CurBasicType,
                IsModify = IsModify,
            };

            SetData(proto);
        }
    }
}
