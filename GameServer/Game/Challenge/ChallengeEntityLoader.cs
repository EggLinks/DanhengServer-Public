using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.Challenge
{
    public class ChallengeEntityLoader(SceneInstance scene, PlayerInstance player) : SceneEntityLoader(scene)
    {
        public PlayerInstance Player = player;

        public override void LoadEntity()
        {
            if (Scene.IsLoaded) return;

            // Get challenge instance
            if (Player.ChallengeManager!.ChallengeInstance == null) return;
            ChallengeInstance instance = Player.ChallengeManager.ChallengeInstance;

            // Setup first stage
            var excel = instance.Excel;
            Scene.FloorInfo!.Groups.TryGetValue(excel.MazeGroupID1, out var groupData);
            if (groupData != null)
            {
                LoadGroup(groupData);
            };

            // Set leave entry
            Scene.LeaveEntityId = instance.IsStory() ? GameConstants.CHALLENGE_STORY_ENTRANCE : GameConstants.CHALLENGE_ENTRANCE;

            foreach (var group in Scene.FloorInfo.Groups.Values)
            {
                // Skip non-server groups
                if (group.LoadSide != Enums.Scene.GroupLoadSideEnum.Server) continue;

                // Dont load the groups that have monsters in them
                if (group.PropList.Count > 0 && group.MonsterList.Count > 0)
                {
                    LoadGroup(group);
                }
            }

            Scene.IsLoaded = true;
        }

        public override EntityMonster? LoadMonster(MonsterInfo info, GroupInfo group, bool sendPacket = false)
        {
            if (info.IsClientOnly || info.IsDelete)
            {
                return null;
            }

            // Get challenge instance
            if (Player.ChallengeManager!.ChallengeInstance == null) return null;
            ChallengeInstance instance = Player.ChallengeManager.ChallengeInstance;

            // Get current stage monster infos
            Dictionary<int, ChallengeConfigExcel.ChallengeMonsterInfo> challengeMonsters;
            if (instance.Excel.MazeGroupID1 == group.Id)
            {
                challengeMonsters = instance.Excel.ChallengeMonsters1;
            }
            else if (instance.Excel.MazeGroupID2 == group.Id)
            {
                challengeMonsters = instance.Excel.ChallengeMonsters2;
            }
            else
            {
                return null;
            }

            // Get challenge monster info
            if (!challengeMonsters.ContainsKey(info.ID)) return null;
            var challengeMonsterInfo = challengeMonsters[info.ID];

            // Get excels from game data
            if (!GameData.NpcMonsterDataData.ContainsKey(challengeMonsterInfo.NpcMonsterId)) return null;
            NPCMonsterDataExcel npcMonsterExcel = GameData.NpcMonsterDataData[challengeMonsterInfo.NpcMonsterId];

            // Create monster from group monster info
            EntityMonster entity = new EntityMonster(scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID, npcMonsterExcel, info);
            entity.EventID = challengeMonsterInfo.EventId;
            entity.CustomStageID = challengeMonsterInfo.EventId;
            Scene.AddEntity(entity, sendPacket);

            return entity;
        }

        public override EntityNpc? LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
        {
            return null;
        }
    }
}
