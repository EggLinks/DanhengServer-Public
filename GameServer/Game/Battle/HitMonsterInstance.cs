using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Battle;

public class HitMonsterInstance(int monsterId, MonsterBattleType battleType)
{
    public int MonsterId { get; set; } = monsterId;
    public MonsterBattleType BattleType { get; set; } = battleType;

    public HitMonsterBattleInfo ToProto()
    {
        return new HitMonsterBattleInfo
        {
            MonsterBattleType = BattleType,
            TargetMonsterEntityId = (uint)MonsterId
        };
    }
}