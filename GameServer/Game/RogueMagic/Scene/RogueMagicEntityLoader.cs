using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.Scene;

public class RogueMagicEntityLoader(SceneInstance scene, PlayerInstance player) : SceneEntityLoader(scene)
{
    public List<RogueMagicRoomTypeEnum> ExistTypes = [];
    public int FinalRoomBossGroup = 500401;
    public int LayerNormalBossGroup1 = 400711;

    public int LayerNormalBossGroup2 = 500301;
    public PlayerInstance Player = player;
    public List<int> RogueDoorPropIds = [1033, 1034, 1035, 1036, 1037, 1000, 1053, 1054, 1055, 1056, 1057];

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        var instance = Player.RogueManager?.GetRogueInstance();
        if (instance is RogueMagicInstance rogue)
        {
            var config = rogue.CurLevel?.CurRoom?.Config;
            if (config == null) return;

            foreach (var group in Scene.FloorInfo?.Groups.Values!)
                if (rogue.CurLevel?.CurRoom?.GetLoadGroupList().Contains(group.Id) == true)
                    await LoadGroup(group);
                else if (group.Category == GroupCategoryEnum.Normal) await LoadGroup(group);
        }

        Scene.IsLoaded = true;
    }

    public override async ValueTask<List<IGameEntity>?> LoadGroup(GroupInfo info, bool forceLoad = false)
    {
        var entityList = new List<IGameEntity>();
        foreach (var npc in info.NPCList)
            try
            {
                if (await LoadNpc(npc, info) is EntityNpc entity) entityList.Add(entity);
            }
            catch
            {
            }

        foreach (var monster in info.MonsterList)
            try
            {
                if (await LoadMonster(monster, info) is EntityMonster entity) entityList.Add(entity);
            }
            catch
            {
            }

        foreach (var prop in info.PropList)
            try
            {
                if (await LoadProp(prop, info) is EntityProp entity) entityList.Add(entity);
            }
            catch
            {
            }

        return entityList;
    }

    public override async ValueTask<EntityNpc?> LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;
        if (!GameData.NpcDataData.ContainsKey(info.NPCID)) return null;

        RogueNpc npc = new(Scene, group, info);
        if (info.NPCID == 3013)
        {
            // generate event
            var instance = await Player.RogueManager!.GetRogueInstance()!.GenerateEvent(npc);
            npc.RogueEvent = instance;
            npc.RogueNpcId = instance.EventId;
            npc.UniqueId = instance.EventUniqueId;
        }

        await Scene.AddEntity(npc, sendPacket);

        return npc;
    }

    public override async ValueTask<EntityMonster?> LoadMonster(MonsterInfo info, GroupInfo group,
        bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;
        var instance = Player.RogueManager?.GetRogueInstance();
        if (instance is not RogueMagicInstance rogueInstance) return null;
        var config = rogueInstance.CurLevel?.CurRoom?.Config;

        if (config == null) return null;

        if (config.RoomType == RogueMagicRoomTypeEnum.Adventure) return await base.LoadMonster(info, group, sendPacket);

        List<MonsterRankEnum> allowedRank = [];

        switch (config.RoomType)
        {
            case RogueMagicRoomTypeEnum.Elite:
                allowedRank.Add(MonsterRankEnum.Elite);
                break;
            default:
                allowedRank.Add(MonsterRankEnum.Minion);
                allowedRank.Add(MonsterRankEnum.MinionLv2);
                break;
        }

        if (allowedRank.Count == 0) return null;

        RogueMonsterExcel? rogueMonster;
        if (config.RoomType == RogueMagicRoomTypeEnum.Boss)
        {
            if (rogueInstance.CurLevel?.LevelIndex == 3)
            {
                var dict = GameData.RogueMonsterGroupData[FinalRoomBossGroup].RogueMonsterListAndWeight;
                var random = new RandomList<int>();
                foreach (var i in dict) random.Add(int.Parse(i.Key), i.Value);

                rogueMonster = GameData.RogueMonsterData[random.GetRandom()];
            }
            else if (rogueInstance.CurLevel?.LevelIndex == 2)
            {
                var dict = GameData.RogueMonsterGroupData[LayerNormalBossGroup2].RogueMonsterListAndWeight;
                var random = new RandomList<int>();
                foreach (var i in dict) random.Add(int.Parse(i.Key), i.Value);

                rogueMonster = GameData.RogueMonsterData[random.GetRandom()];
            }
            else
            {
                var dict = GameData.RogueMonsterGroupData[LayerNormalBossGroup1].RogueMonsterListAndWeight;
                var random = new RandomList<int>();
                foreach (var i in dict) random.Add(int.Parse(i.Key), i.Value);

                rogueMonster = GameData.RogueMonsterData[random.GetRandom()];
            }
        }
        else
        {
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
                CustomStageID = rogueMonster.EventID,
                RogueMonsterId = rogueMonster.RogueMonsterID
            };

        await Scene.AddEntity(entity, sendPacket);

        return entity;
    }

    public override async ValueTask<EntityProp?> LoadProp(PropInfo info, GroupInfo group, bool sendPacket = false)
    {
        var room = Player.RogueMagicManager?.RogueMagicInstance?.CurLevel?.CurRoom;
        if (room == null) return null;

        var magic = Player.RogueMagicManager?.RogueMagicInstance;
        if (magic == null) return null;

        GameData.MazePropData.TryGetValue(info.PropID, out var propExcel);
        if (propExcel == null) return null;

        if (info.PropID == 1049) return null; // gamble machine

        var prop = new RogueProp(Scene, propExcel, group, info);

        if (RogueDoorPropIds.Contains(prop.PropInfo.PropID))
        {
            if (magic.CurLevel?.LayerId == magic.Levels.Last().Key &&
                magic.CurLevel?.Rooms.Last().RoomIndex == room.RoomIndex) // last room
            {
                // exit
                if (prop.InstId != 300002) return null; // not center door
                prop.CustomPropID = 1053;
            }
            else
            {
                do // find next room
                {
                    RandomList<RogueMagicRoomTypeEnum> roomTypes = new();
                    foreach (var kv in Player.RogueMagicManager!.RogueMagicInstance!.RoomTypeWeight.Where(kv =>
                                 kv.Value != 0)) roomTypes.Add(kv.Key, kv.Value);

                    var nextRoom = roomTypes.GetRandom();

                    if (room.LevelInstance.Rooms.Last().RoomIndex - 1 == room.RoomIndex) // boss only
                    {
                        if (prop.InstId != 300002) return null; // not center door
                        nextRoom = RogueMagicRoomTypeEnum.Boss;
                    }
                    else if (room.LevelInstance.Rooms.Last().RoomIndex - 2 == room.RoomIndex &&
                             room.LevelInstance.LevelIndex == 3) // respite only
                    {
                        if (prop.InstId != 300002) return null; // not center door
                        nextRoom = RogueMagicRoomTypeEnum.Reforge;
                    }
                    else
                    {
                        if (ExistTypes.Contains(nextRoom)) continue;
                        ExistTypes.Add(nextRoom);
                    }

                    prop.CustomPropID = nextRoom switch // door style
                    {
                        RogueMagicRoomTypeEnum.Event => 1055,
                        RogueMagicRoomTypeEnum.Wealth => 1055,
                        RogueMagicRoomTypeEnum.Shop => 1055,
                        RogueMagicRoomTypeEnum.Reward => 1055,
                        RogueMagicRoomTypeEnum.Adventure => 1055,
                        RogueMagicRoomTypeEnum.Reforge => 1054,
                        _ => 1054
                    };


                    if (room.LevelInstance.Rooms.Last() == room) // last room
                        prop.EnterNextLayer = true;
                    prop.MagicRoomType = nextRoom;
                    prop.IsMagicRogue = true;

                    break;
                } while (true);
            }

            await prop.SetState(PropStateEnum.Open);
        }
        else if (prop.PropInfo.PropID == 1038)
        {
            var p = new RogueWorkbenchProp(Scene, propExcel, group, info)
            {
                WorkbenchId = 105
            };
            var workbenchExcel = GameData.RogueTournWorkbenchData.GetValueOrDefault(p.WorkbenchId);
            if (workbenchExcel != null)
                foreach (var funcExcel in workbenchExcel.Funcs)
                    p.WorkbenchFuncs.Add(new RogueWorkbenchFunc(funcExcel));

            prop = p;
            await prop.SetState(info.State);
        }
        else
        {
            await prop.SetState(info.State);
        }

        await Scene.AddEntity(prop, sendPacket);

        return prop;
    }
}