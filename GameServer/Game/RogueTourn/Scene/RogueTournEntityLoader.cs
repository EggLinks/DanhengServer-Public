using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;

public class RogueTournEntityLoader(SceneInstance scene, PlayerInstance player) : SceneEntityLoader(scene)
{
    public List<RogueTournRoomTypeEnum> ExistTypes = [];
    public List<int> FinalRoomBossIds = [3007091, 3007101, 3007111, 3007121, 3007131, 3007141];
    public List<int> LayerNormalBossIds = [3007011, 3007021, 3007031, 3007041, 3007051, 3007061, 3007071, 3007081];
    public PlayerInstance Player = player;
    public List<int> RogueDoorPropIds = [1033, 1034, 1035, 1036, 1037, 1000];

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        var instance = Player.RogueManager?.GetRogueInstance();
        if (instance is RogueTournInstance rogue)
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
        if (instance is not RogueTournInstance rogueInstance) return null;
        var config = rogueInstance.CurLevel?.CurRoom?.Config;

        if (config == null) return null;

        List<MonsterRankEnum> allowedRank = [];

        switch (config.RoomType)
        {
            case RogueTournRoomTypeEnum.Elite:
                allowedRank.Add(MonsterRankEnum.Elite);
                break;
            default:
                allowedRank.Add(MonsterRankEnum.Minion);
                allowedRank.Add(MonsterRankEnum.MinionLv2);
                break;
        }

        if (allowedRank.Count == 0) return null;

        RogueMonsterExcel? rogueMonster;
        if (config.RoomType == RogueTournRoomTypeEnum.Boss)
        {
            if (rogueInstance.CurLevel?.LevelIndex == 3)
                rogueMonster = GameData.RogueMonsterData[FinalRoomBossIds.RandomElement()];
            else
                rogueMonster = GameData.RogueMonsterData[LayerNormalBossIds.RandomElement()];
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
        var room = Player.RogueTournManager?.RogueTournInstance?.CurLevel?.CurRoom;
        if (room == null) return null;

        GameData.MazePropData.TryGetValue(info.PropID, out var propExcel);
        if (propExcel == null) return null;

        if (info.PropID == 1049) return null; // gamble machine

        var prop = new RogueProp(Scene, propExcel, group, info);

        if (RogueDoorPropIds.Contains(prop.PropInfo.PropID))
        {
            if (room is { RoomIndex: 4, LevelInstance.LevelIndex: 3 }) // last room
                // exit
                prop.CustomPropID = 1033;
            else
                do // find next room
                {
                    RandomList<RogueTournRoomTypeEnum> roomTypes = new();
                    foreach (var kv in Player.RogueTournManager!.RogueTournInstance!.RoomTypeWeight.Where(kv =>
                                 kv.Value != 0)) roomTypes.Add(kv.Key, kv.Value);

                    var nextRoom = roomTypes.GetRandom();

                    if (room.LevelInstance.Rooms.Last().RoomIndex - 1 == room.RoomIndex) // boss only
                    {
                        if (prop.InstId != 300002) return null; // not center door
                        nextRoom = RogueTournRoomTypeEnum.Boss;
                    }
                    else if (room.LevelInstance.Rooms.Last().RoomIndex - 2 == room.RoomIndex &&
                             room.LevelInstance.LevelIndex == 3) // respite only
                    {
                        if (prop.InstId != 300002) return null; // not center door
                        nextRoom = RogueTournRoomTypeEnum.Respite;
                    }
                    else
                    {
                        if (ExistTypes.Contains(nextRoom)) continue;
                        ExistTypes.Add(nextRoom);
                    }

                    prop.CustomPropID = nextRoom switch // door style
                    {
                        RogueTournRoomTypeEnum.Event => 1035,
                        RogueTournRoomTypeEnum.Coin => 1035,
                        RogueTournRoomTypeEnum.Shop => 1035,
                        RogueTournRoomTypeEnum.Reward => 1035,
                        RogueTournRoomTypeEnum.Adventure => 1035,
                        RogueTournRoomTypeEnum.Hidden => 1037,
                        RogueTournRoomTypeEnum.Respite => 1034,
                        _ => 1034
                    };


                    if (room.LevelInstance.Rooms.Last() == room) // last room
                        prop.EnterNextLayer = true;
                    prop.RoomType = nextRoom;
                    prop.IsTournRogue = true;

                    break;
                } while (true);

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