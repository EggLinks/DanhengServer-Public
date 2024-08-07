using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.MapRotation;

public class PacketDeployRotaterScRsp : BasePacket
{
    public PacketDeployRotaterScRsp(RotaterData rotaterData, int curNum, int maxNum) : base(CmdIds.DeployRotaterScRsp)
    {
        var proto = new DeployRotaterScRsp
        {
            EnergyInfo = new RotaterEnergyInfo
            {
                MaxNum = (uint)maxNum,
                CurNum = (uint)curNum
            },
            RotaterData = rotaterData
        };

        SetData(proto);
    }
}