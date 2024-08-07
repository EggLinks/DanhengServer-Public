using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketFinishCosumeItemMissionScRsp : BasePacket
{
    public PacketFinishCosumeItemMissionScRsp(uint subMissionId) : base(CmdIds.FinishCosumeItemMissionScRsp)
    {
        var proto = new FinishCosumeItemMissionScRsp
        {
            SubMissionId = subMissionId
        };

        SetData(proto);
    }

    public PacketFinishCosumeItemMissionScRsp() : base(CmdIds.FinishCosumeItemMissionScRsp)
    {
        var proto = new FinishCosumeItemMissionScRsp
        {
            Retcode = 1
        };
        SetData(proto);
    }
}