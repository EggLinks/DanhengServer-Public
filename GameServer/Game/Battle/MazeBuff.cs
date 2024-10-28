using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Battle;

public class MazeBuff(int buffID, int buffLevel, int owner)
{
    public MazeBuff(SceneBuff buff) : this(buff.BuffId, buff.BuffLevel, 0)
    {
        Duration = buff.Duration;
        OwnerAvatarId = buff.OwnerAvatarId;
    }

    public int BuffID { get; } = buffID;
    public int BuffLevel { get; } = buffLevel;
    public int OwnerIndex { get; private set; } = owner;
    public int OwnerAvatarId { get; set; } = -1;
    public int? WaveFlag { get; set; } = null;
    public int Duration { get; private set; } = -1;
    public Dictionary<string, float> DynamicValues { get; } = [];

    public BattleBuff ToProto(BattleInstance battle)
    {
        return ToProto(battle, WaveFlag ?? -1);
    }

    public BattleBuff ToProto(BattleInstance instance, int waveFlag)
    {
        var buffInfo = new BattleBuff
        {
            Id = (uint)BuffID,
            Level = (uint)BuffLevel,
            OwnerIndex = (uint)OwnerIndex,
            WaveFlag = (uint)waveFlag
        };

        foreach (var item in DynamicValues) buffInfo.DynamicValues.Add(item.Key, item.Value);

        if (OwnerAvatarId != -1)
        {
            buffInfo.OwnerIndex = (uint)instance.Lineup.BaseAvatars!.FindIndex(x => x.BaseAvatarId == OwnerAvatarId);
            OwnerIndex = (int)buffInfo.OwnerIndex;
        }

        if (OwnerIndex != -1)
            buffInfo.TargetIndexList.Add((uint)OwnerIndex);

        return buffInfo;
    }
}