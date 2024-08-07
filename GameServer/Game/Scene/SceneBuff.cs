using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Scene;

public class SceneBuff(int buffID, int buffLevel, int owner, int duration = -1)
{
    public Dictionary<string, float> DynamicValues = [];
    public int BuffID { get; } = buffID;
    public int BuffLevel { get; } = buffLevel;
    public int OwnerAvatarId { get; } = owner;

    public int Duration { get; set; } = duration;
    public long CreatedTime { get; set; } = Extensions.GetUnixMs();

    public bool IsExpired()
    {
        if (Duration < 0)
            return false; // Permanent buff
        return Extensions.GetUnixMs() - CreatedTime >= Duration;
    }

    public BuffInfo ToProto()
    {
        var buffInfo = new BuffInfo
        {
            BuffId = (uint)BuffID,
            Level = (uint)BuffLevel,
            BaseAvatarId = (uint)OwnerAvatarId,
            AddTimeMs = (ulong)CreatedTime,
            LifeTime = (ulong)Duration
        };

        return buffInfo;
    }
}