using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneCastSkillScRsp : BasePacket
{
    public PacketSceneCastSkillScRsp(uint castEntityId) : base(CmdIds.SceneCastSkillScRsp)
    {
        var proto = new SceneCastSkillScRsp
        {
            CastEntityId = castEntityId
        };

        SetData(proto);
    }

    public PacketSceneCastSkillScRsp(uint castEntityId, BattleInstance battle) : base(CmdIds.SceneCastSkillScRsp)
    {
        var proto = new SceneCastSkillScRsp
        {
            CastEntityId = castEntityId,
            BattleInfo = battle.ToProto()
        };

        SetData(proto);
    }
}