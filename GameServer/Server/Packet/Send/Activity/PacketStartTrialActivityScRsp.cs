using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Activity;

public class PacketStartTrialActivityScRsp : BasePacket
{
    public PacketStartTrialActivityScRsp(uint stageId) : base(CmdIds.StartTrialActivityScRsp)
    {
        var proto = new StartTrialActivityScRsp
        {
            StageId = stageId
        };

        SetData(proto);
    }
}