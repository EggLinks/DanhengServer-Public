using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketPlayerKickOutScNotify : BasePacket
    {
        public PacketPlayerKickOutScNotify() : base(CmdIds.PlayerKickOutScNotify)
        {
            var proto = new PlayerKickOutScNotify()
            {
                KickType = KickType.KickSqueezed,
            };
            SetData(proto);
        }
    }
}
