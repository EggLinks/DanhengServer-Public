using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Scene;

public class SceneBuff(int buffId, int buffLevel, int owner, int duration = -1)
{
    public Dictionary<string, float> DynamicValues { get; set; } = [];
    public int BuffId { get; } = buffId;
    public int BuffLevel { get; } = buffLevel;
    public int OwnerAvatarId { get; } = owner;

    public int Duration { get; set; } = duration;
    public long CreatedTime { get; set; } = Extensions.GetUnixMs();

    public int SummonUnitEntityId { get; set; } = 0;

    public bool IsExpired()
    {
        if (Duration < 0)
            return false; // Permanent buff
        return Extensions.GetUnixMs() - CreatedTime >= Duration * 1000;
    }

    public BuffInfo ToProto()
    {
        var buffInfo = new BuffInfo
        {
            BuffId = (uint)BuffId,
            Level = (uint)BuffLevel,
            BaseAvatarId = (uint)OwnerAvatarId,
            AddTimeMs = (ulong)CreatedTime,
            LifeTime = Duration,
            BuffSummonEntityId = (uint)SummonUnitEntityId,
            Count = 1
        };

        foreach (var item in DynamicValues)
            buffInfo.DynamicValues.Add(item.Key, item.Value);

        return buffInfo;
    }
}