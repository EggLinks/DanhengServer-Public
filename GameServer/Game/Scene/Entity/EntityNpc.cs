using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Scene.Entity;

public class EntityNpc(SceneInstance scene, GroupInfo group, NpcInfo npcInfo) : IGameEntity
{
    public SceneInstance Scene { get; set; } = scene;
    public Position Position { get; set; } = npcInfo.ToPositionProto();
    public Position Rotation { get; set; } = npcInfo.ToRotationProto();
    public int NpcId { get; set; } = npcInfo.NPCID;
    public int InstId { get; set; } = npcInfo.ID;
    public int EntityID { get; set; }
    public int GroupID { get; set; } = group.Id;

    public async ValueTask AddBuff(SceneBuff buff)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }

    public async ValueTask ApplyBuff(BattleInstance instance)
    {
        await System.Threading.Tasks.Task.CompletedTask;
    }

    public virtual SceneEntityInfo ToProto()
    {
        SceneNpcInfo npc = new()
        {
            NpcId = (uint)NpcId
        };

        return new SceneEntityInfo
        {
            EntityId = (uint)EntityID,
            GroupId = (uint)GroupID,
            Motion = new MotionInfo
            {
                Pos = Position.ToProto(),
                Rot = Rotation.ToProto()
            },
            InstId = (uint)InstId,
            Npc = npc
        };
    }
}