using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.MapRotation;

public class PacketEnterMapRotationRegionScRsp : BasePacket
{
    public PacketEnterMapRotationRegionScRsp(MotionInfo motion) : base(CmdIds.EnterMapRotationRegionScRsp)
    {
        var proto = new EnterMapRotationRegionScRsp
        {
            Motion = motion,
            EnergyInfo = new RotaterEnergyInfo
            {
                CurNum = 5,
                MaxNum = 5
            }
        };

        SetData(proto);
    }
}