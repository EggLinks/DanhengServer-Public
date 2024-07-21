using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission
{
    public class StoryLineEntityLoader(SceneInstance scene) : SceneEntityLoader(scene)
    {
        public List<int> LoadGroups = [];
        public override void LoadEntity()
        {
            if (Scene.IsLoaded) return;

            var storyId = Scene.Player.StoryLineManager?.StoryLineData.CurStoryLineId ?? 0;
            if (storyId == 0) return;

            GameData.StoryLineFloorDataData.TryGetValue(storyId, out var floorData);
            if (floorData == null) return;

            floorData.TryGetValue(Scene.FloorInfo?.FloorID ?? 0, out var floorInfo);
            if (floorInfo == null) return;

            var dim = Scene?.FloorInfo?.DimensionList.Find(d => d.ID == floorInfo.DimensionID);
            if (dim == null) return;

            LoadGroups.AddRange(dim.GroupIDList);

            foreach (var group in Scene?.FloorInfo?.Groups.Values!)  // Sanity check in SceneInstance
            {
                if (group.LoadSide == GroupLoadSideEnum.Client)
                {
                    continue;
                }
                if (group.GroupName.Contains("TrainVisitor"))
                {
                    continue;
                }
                LoadGroup(group);
            }
            Scene.IsLoaded = true;
        }

        public override List<IGameEntity>? LoadGroup(GroupInfo info, bool forceLoad = false)
        {
            if (!LoadGroups.Contains(info.Id)) return null;

            if (Scene.Entities.Values.ToList().FindIndex(x => x.GroupID == info.Id) != -1)  // check if group is already loaded
            {
                return null;
            }

            // load
            Scene.Groups.Add(info.Id);

            var entityList = new List<IGameEntity>();
            foreach (var npc in info.NPCList)
            {
                try
                {
                    if (LoadNpc(npc, info) is EntityNpc entity)
                    {
                        entityList.Add(entity);
                    }
                }
                catch { }
            }

            foreach (var monster in info.MonsterList)
            {
                try
                {
                    if (LoadMonster(monster, info) is EntityMonster entity)
                    {
                        entityList.Add(entity);
                    }
                }
                catch { }
            }

            foreach (var prop in info.PropList)
            {
                try
                {
                    if (LoadProp(prop, info) is EntityProp entity)
                    {
                        entityList.Add(entity);
                    }
                }
                catch { }
            }

            return entityList;
        }

        public override void SyncEntity()
        {
            return;
        }
    }
}
