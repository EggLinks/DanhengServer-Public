using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Scene.Entity;

public class EntityMonster(
    SceneInstance scene,
    Position pos,
    Position rot,
    int GroupID,
    int InstID,
    NPCMonsterDataExcel excel,
    MonsterInfo info) : IGameEntity
{
    public Position Position { get; set; } = pos;
    public Position Rotation { get; set; } = rot;
    public int InstID { get; set; } = InstID;
    public SceneInstance Scene { get; set; } = scene;
    public NPCMonsterDataExcel MonsterData { get; set; } = excel;
    public MonsterInfo Info { get; set; } = info;
    public List<SceneBuff> BuffList { get; set; } = [];
    public SceneBuff? TempBuff { get; set; }
    public bool IsAlive { get; private set; } = true;
    public bool IsInSummonUnit { get; set; } = false;

    public int EventID { get; set; } = info.EventID;
    public int CustomStageID { get; set; } = 0;

    public int RogueMonsterId { get; set; } = 0;
    public int CustomLevel { get; set; } = 0;
    public int EntityID { get; set; } = 0;
    public int GroupID { get; set; } = GroupID;

    public async ValueTask AddBuff(SceneBuff buff)
    {
        var oldBuff = BuffList.Find(x => x.BuffId == buff.BuffId);
        if (oldBuff != null) BuffList.Remove(oldBuff);
        BuffList.Add(buff);
        await Scene.Player.SendPacket(new PacketSyncEntityBuffChangeListScNotify(this, buff));
    }

    public async ValueTask ApplyBuff(BattleInstance instance)
    {
        if (TempBuff != null)
        {
            instance.Buffs.Add(new MazeBuff(TempBuff));
            TempBuff = null;
        }

        if (BuffList.Count == 0) return;

        foreach (var buff in BuffList)
        {
            if (buff.IsExpired()) continue;
            instance.Buffs.Add(new MazeBuff(buff));
        }

        await Scene.Player.SendPacket(new PacketSyncEntityBuffChangeListScNotify(this, BuffList));

        BuffList.Clear();
    }

    public SceneEntityInfo ToProto()
    {
        var proto = new SceneEntityInfo
        {
            EntityId = (uint)EntityID,
            GroupId = (uint)GroupID,
            InstId = (uint)InstID,
            Motion = new MotionInfo
            {
                Pos = Position.ToProto(),
                Rot = Rotation.ToProto()
            },
            NpcMonster = new SceneNpcMonsterInfo
            {
                EventId = (uint)EventID,
                MonsterId = (uint)MonsterData.ID,
                WorldLevel = (uint)Scene.Player.Data.WorldLevel
            }
        };

        if (RogueMonsterId > 0)
            proto.NpcMonster.ExtraInfo = new NpcMonsterExtraInfo
            {
                RogueGameInfo = new NpcMonsterRogueInfo
                {
                    RogueMonsterId = (uint)RogueMonsterId,
                    Level = (uint)CustomLevel
                }
            };

        return proto;
    }

    public async ValueTask RemoveBuff(int buffId)
    {
        var buff = BuffList.Find(x => x.BuffId == buffId);
        if (buff == null) return;

        BuffList.Remove(buff);
        await Scene.Player.SendPacket(new PacketSyncEntityBuffChangeListScNotify(this, [buff]));
    }

    public int GetStageId()
    {
        if (CustomStageID > 0) return CustomStageID;
        return Info.EventID;
    }

    public async ValueTask<List<ItemData>> Kill(bool sendPacket = true)
    {
        IsAlive = false;

        GameData.MonsterDropData.TryGetValue(MonsterData.ID * 10 + Scene.Player.Data.WorldLevel, out var dropData);
        if (dropData == null) return [];
        var dropItems = dropData.CalculateDrop();
        await Scene.Player.InventoryManager!.AddItems(dropItems, sendPacket);

        // TODO: Rogue support
        // call mission handler
        await Scene.Player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.KillMonster, this);
        await Scene.RemoveEntity(this);
        return dropItems;
    }
}