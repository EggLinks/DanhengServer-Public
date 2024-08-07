using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.MapRotation;

public class PacketInteractChargerScRsp : BasePacket
{
    public PacketInteractChargerScRsp(ChargerInfo chargerInfo) : base(CmdIds.InteractChargerScRsp)
    {
        var proto = new InteractChargerScRsp
        {
            ChargerInfo = chargerInfo
        };

        SetData(proto);
    }
}