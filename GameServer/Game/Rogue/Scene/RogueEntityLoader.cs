using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Scene;

public class RogueEntityLoader(SceneInstance scene, PlayerInstance player) : SceneEntityLoader(scene)
{
    public List<int> NextRoomIds = [];
    public PlayerInstance Player = player;
    public List<int> RogueDoorPropIds = [1000, 1021, 1022, 1023];

    public override async ValueTask LoadEntity()
    {
        if (Scene.IsLoaded) return;

        var instance = Player.RogueManager?.GetRogueInstance();
        if (instance is RogueInstance rogue)
        {
            var excel = rogue.CurRoom?.Excel;
            if (excel == null) return;

            foreach (var group in excel.GroupWithContent)
            {
                Scene.FloorInfo!.Groups.TryGetValue(group.Key, out var groupData);
                if (groupData == null) continue;
                await LoadGroup(groupData);
            }
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
            if (instance != null)
            {
                npc.RogueEvent = instance;
                npc.RogueNpcId = instance.EventId;
                npc.UniqueId = instance.EventUniqueId;
            }
        }

        await Scene.AddEntity(npc, sendPacket);

        return npc;
    }

    public override async ValueTask<EntityMonster?> LoadMonster(MonsterInfo info, GroupInfo group,
        bool sendPacket = false)
    {
        if (info.IsClientOnly || info.IsDelete) return null;
        var instance = Player.RogueManager?.GetRogueInstance();
        if (instance is RogueInstance rogueInstance)
        {
            var room = rogueInstance.CurRoom;
            if (room == null) return null;

            var content = room.Excel?.GroupWithContent[group.Id];
            if (content == null) return null;

            GameData.RogueMonsterData.TryGetValue((int)(content * 10 + 1), out var rogueMonster);
            if (rogueMonster == null) return null;

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

        return null;
    }

    public override async ValueTask<EntityProp?> LoadProp(PropInfo info, GroupInfo group, bool sendPacket = false)
    {
        var room = Player.RogueManager?.RogueInstance?.CurRoom;
        if (room == null) return null;
        var excel = room.Excel;
        if (excel == null) return null;

        GameData.MazePropData.TryGetValue(info.PropID, out var propExcel);
        if (propExcel == null) return null;

        var prop = new RogueProp(Scene, propExcel, group, info);

        if (RogueDoorPropIds.Contains(prop.PropInfo.PropID))
        {
            var index = NextRoomIds.Count;
            var nextSiteIds = room.NextSiteIds;
            if (nextSiteIds.Count == 0)
            {
                // exit
                prop.CustomPropID = 1000;
            }
            else
            {
                index = Math.Min(index, nextSiteIds.Count - 1); // Sanity check
                var nextRoom = Player.RogueManager?.RogueInstance?.RogueRooms[nextSiteIds[index]];
                prop.NextSiteID = nextSiteIds[index];
                prop.NextRoomID = nextRoom!.Excel?.RogueRoomID ?? 0;
                NextRoomIds.Add(prop.NextRoomID);

                prop.CustomPropID = nextRoom!.Excel!.RogueRoomType switch // door style
                {
                    3 => 1022,
                    8 => 1022,
                    5 => 1023,
                    _ => 1021
                };
            }

            await prop.SetState(PropStateEnum.Open);
        }
        else
        {
            await prop.SetState(info.State);
        }

        await Scene.AddEntity(prop, sendPacket);

        return prop;
    }
}