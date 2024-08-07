using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneCastSkillCostMpScRsp : BasePacket
{
    public PacketSceneCastSkillCostMpScRsp(int entityId) : base(CmdIds.SceneCastSkillCostMpScRsp)
    {
        var proto = new SceneCastSkillCostMpScRsp
        {
            CastEntityId = (uint)entityId
        };

        SetData(proto);
    }
}