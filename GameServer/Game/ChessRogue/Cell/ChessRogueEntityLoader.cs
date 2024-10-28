using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;

public class ChessRogueEntityLoader(SceneInstance scene) : SceneEntityLoader(scene)
{
    public ChessRogueInstance instance = scene.Player.ChessRogueManager!.RogueInstance!;

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;
        LoadGroups.AddRange(Scene.FloorInfo?.Groups.Keys ?? []);
        foreach (var group in Scene.FloorInfo?.Groups.Values!) // Sanity check in SceneInstance
        {
            if (group.LoadSide == GroupLoadSideEnum.Client) continue;
            if (instance.CurCell!.GetLoadGroupList().Contains(group.Id))
                await LoadGroup(group);
            else if (group.Category == GroupCategoryEnum.Normal) await LoadGroup(group);
        }

        Scene.IsLoaded = true;
    }


    public override async ValueTask<EntityNpc?> LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;
        if (!GameData.NpcDataData.ContainsKey(info.NPCID)) return null;

        RogueNpc npc = new(Scene, group, info);
        if (info.NPCID == 3013)
        {
            // generate event
            var i = await instance.GenerateEvent(npc);
            if (i != null)
            {
                npc.RogueEvent = i;
                npc.RogueNpcId = i.EventId;
                npc.UniqueId = i.EventUniqueId;
            }
        }

        await Scene.AddEntity(npc, sendPacket);

        return npc;
    }

    public override async ValueTask<EntityMonster?> LoadMonster(MonsterInfo info, GroupInfo group,
        bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;

        var room = instance.CurCell;
        if (room == null) return null;
        int monsterId;
        RogueMonsterExcel? rogueMonster;
        if (room.SelectMonsterId > 0)
        {
            monsterId = room.SelectMonsterId;

            GameData.RogueMonsterData.TryGetValue(monsterId * 10 + 1, out rogueMonster);
            if (rogueMonster == null) return null;
        }
        else
        {
            List<MonsterRankEnum> allowedRank = [];
            if (room.BlockType == RogueDLCBlockTypeEnum.MonsterElite)
            {
                allowedRank.Add(MonsterRankEnum.Elite);
            }
            else
            {
                allowedRank.Add(MonsterRankEnum.Minion);
                allowedRank.Add(MonsterRankEnum.MinionLv2);
            }

            NPCMonsterDataExcel? data;
            do
            {
                rogueMonster = GameData.RogueMonsterData.Values.ToList().RandomElement();
                GameData.NpcMonsterDataData.TryGetValue(rogueMonster.NpcMonsterID, out data);
            } while (data == null || !allowedRank.Contains(data.Rank));
        }

        GameData.NpcMonsterDataData.TryGetValue(rogueMonster.NpcMonsterID, out var excel);
        if (excel == null) return null;

        EntityMonster entity =
            new(Scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID, excel, info)
            {
                EventID = rogueMonster.EventID,
                CustomStageID = rogueMonster.EventID
            };

        await Scene.AddEntity(entity, sendPacket);

        return entity;
    }

    public override async ValueTask<EntityProp?> LoadProp(PropInfo info, GroupInfo group, bool sendPacket = false)
    {
        GameData.MazePropData.TryGetValue(info.PropID, out var propExcel);
        if (propExcel == null) return null;

        var prop = new RogueProp(Scene, propExcel, group, info);

        if (prop.PropInfo.PropID == 1026)
        {
            await prop.SetState(PropStateEnum.CustomState02);
            prop.IsChessRogue = true;
            if (instance.CurCell!.BlockType == RogueDLCBlockTypeEnum.MonsterBoss ||
                instance.CurCell.BlockType == RogueDLCBlockTypeEnum.MonsterNousBoss ||
                instance.CurCell.BlockType == RogueDLCBlockTypeEnum.MonsterSwarmBoss)
            {
                await prop.SetState(PropStateEnum.CustomState04);
                if (instance.CurCell!.BlockType != RogueDLCBlockTypeEnum.MonsterBoss) prop.IsLastRoom = true;
            }
        }
        else
        {
            await prop.SetState(info.State);
        }

        await Scene.AddEntity(prop, sendPacket);

        return null;
    }
}