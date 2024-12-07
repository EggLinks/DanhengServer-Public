using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneCastSkillScRsp : BasePacket
{
    public PacketSceneCastSkillScRsp(uint castEntityId, List<HitMonsterInstance> hitMonsters) : base(
        CmdIds.SceneCastSkillScRsp)
    {
        var proto = new SceneCastSkillScRsp
        {
            CastEntityId = castEntityId
        };

        foreach (var hitMonster in hitMonsters) proto.MonsterBattleInfo.Add(hitMonster.ToProto());

        SetData(proto);
    }

    public PacketSceneCastSkillScRsp(uint castEntityId, BattleInstance battle, List<HitMonsterInstance> hitMonsters) :
        base(CmdIds.SceneCastSkillScRsp)
    {
        var proto = new SceneCastSkillScRsp
        {
            CastEntityId = castEntityId,
            BattleInfo = battle.ToProto()
        };

        foreach (var hitMonster in hitMonsters) proto.MonsterBattleInfo.Add(hitMonster.ToProto());

        SetData(proto);
    }
}