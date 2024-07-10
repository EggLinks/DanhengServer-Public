using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EggLink.DanhengServer.Game.Rogue;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Data.Excel;

namespace EggLink.DanhengServer.Game.ChessRogue.Cell
{
    public class ChessRogueEntityLoader(SceneInstance scene) : SceneEntityLoader(scene)
    {
        public ChessRogueInstance instance = scene.Player.ChessRogueManager!.RogueInstance!;

        public override void LoadEntity()
        {
            if (Scene.IsLoaded) return;

            foreach (var group in Scene?.FloorInfo?.Groups.Values!)  // Sanity check in SceneInstance
            {
                if (group.LoadSide == GroupLoadSideEnum.Client)
                {
                    continue;
                }
                if (instance.CurCell!.GetLoadGroupList().Contains(group.Id))
                {
                    LoadGroup(group);
                }
                else if (group.Category == GroupCategoryEnum.Normal)
                {
                    LoadGroup(group);
                }
            }
            Scene.IsLoaded = true;
        }


        public override EntityNpc? LoadNpc(NpcInfo info, GroupInfo group, bool sendPacket = false)
        {
            if (info.IsClientOnly || info.IsDelete)
            {
                return null;
            }
            if (!GameData.NpcDataData.ContainsKey(info.NPCID))
            {
                return null;
            }

            bool hasDuplicateNpcId = false;
            foreach (IGameEntity entity in Scene.Entities.Values)
            {
                if (entity is EntityNpc eNpc && eNpc.NpcId == info.NPCID)
                {
                    hasDuplicateNpcId = true;
                    break;
                }
            }
            if (hasDuplicateNpcId)
            {
                return null;
            }

            RogueNpc npc = new(Scene, group, info);
            if (info.NPCID == 3013)
            {
                // generate event
                var eventInstance = instance.GenerateEvent(npc);
                if (instance != null)
                {
                    npc.RogueEvent = eventInstance;
                    npc.RogueNpcId = eventInstance.EventId;
                    npc.UniqueId = eventInstance.EventUniqueId;
                }
            }
            Scene.AddEntity(npc, sendPacket);

            return npc;
        }

        public override EntityMonster? LoadMonster(MonsterInfo info, GroupInfo group, bool sendPacket = false)
        {
            if (info.IsClientOnly || info.IsDelete)
            {
                return null;
            }

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
                if (room.CellType == RogueDLCBlockTypeEnum.MonsterElite)
                {
                    allowedRank.Add(MonsterRankEnum.Elite);
                } else
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

            EntityMonster entity = new(Scene, info.ToPositionProto(), info.ToRotationProto(), group.Id, info.ID, excel, info)
            {
                EventID = rogueMonster.EventID,
                CustomStageID = rogueMonster.EventID
            };

            Scene.AddEntity(entity, sendPacket);

            return entity;
        }

        public override EntityProp? LoadProp(PropInfo info, GroupInfo group, bool sendPacket = false)
        {
            GameData.MazePropData.TryGetValue(info.PropID, out var propExcel);
            if (propExcel == null)
            {
                return null;
            }

            var prop = new RogueProp(Scene, propExcel, group, info);

            if (prop.PropInfo.PropID == 1026)
            {
                prop.SetState(PropStateEnum.CustomState02);
                prop.IsChessRogue = true;
                if (instance.CurCell!.CellType == RogueDLCBlockTypeEnum.MonsterBoss || instance.CurCell.CellType == RogueDLCBlockTypeEnum.MonsterNousBoss || instance.CurCell.CellType == RogueDLCBlockTypeEnum.MonsterSwarmBoss)
                {
                    prop.SetState(PropStateEnum.CustomState04);
                    if (instance.CurCell!.CellType != RogueDLCBlockTypeEnum.MonsterBoss)
                    {
                        prop.IsLastRoom = true;
                    }
                }
            }
            else
            {
                prop.SetState(info.State);
            }

            Scene.AddEntity(prop, sendPacket);

            return null;
        }
    }
}
