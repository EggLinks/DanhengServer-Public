using EggLink.DanhengServer.Data.Config.SummonUnit;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Scene.Entity;

public class EntitySummonUnit : IGameEntity
{
    public int CreateAvatarEntityId { get; set; } = 0;
    public int AttachEntityId { get; set; } = 0;
    public int CreateAvatarId { get; set; } = 0;
    public long CreateTimeMs { get; set; } = Extensions.GetUnixMs();
    public int LifeTimeMs { get; set; } = -1;
    public int SummonUnitId { get; set; } = 0;
    public MotionInfo Motion { get; set; } = new();

    public List<UnitCustomTriggerConfigInfo> TriggerList { get; set; } = [];
    public int EntityID { get; set; }
    public int GroupID { get; set; } = 0;

    public async ValueTask AddBuff(SceneBuff buff)
    {
        await ValueTask.CompletedTask;
    }

    public async ValueTask ApplyBuff(BattleInstance instance)
    {
        await ValueTask.CompletedTask;
    }

    public SceneEntityInfo ToProto()
    {
        return new SceneEntityInfo
        {
            EntityId = (uint)EntityID,
            GroupId = (uint)GroupID,
            Motion = Motion,
            SummonUnit = new SceneSummonUnitInfo
            {
                CasterEntityId = (uint)CreateAvatarEntityId,
                AttachEntityId = (uint)AttachEntityId,
                CreateTimeMs = (ulong)CreateTimeMs,
                LifeTimeMs = LifeTimeMs,
                SummonUnitId = (uint)SummonUnitId,
                TriggerNameList = { TriggerList.Select(x => x.TriggerName) }
            }
        };
    }
}