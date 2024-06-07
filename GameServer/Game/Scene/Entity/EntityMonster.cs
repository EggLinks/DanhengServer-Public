using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Scene.Entity
{
    public class EntityMonster(SceneInstance scene, Position pos, Position rot, int GroupID, int InstID, NPCMonsterDataExcel excel, MonsterInfo info) : IGameEntity
    {
        public int EntityID { get; set; } = 0;
        public int GroupID { get; set; } = GroupID;
        public Position Position { get; set; } = pos;
        public Position Rotation { get; set; } = rot;
        public int InstID { get; set; } = InstID;
        public SceneInstance Scene { get; set; } = scene;
        public NPCMonsterDataExcel MonsterData { get; set; } = excel;
        public MonsterInfo Info { get; set; } = info;
        public List<SceneBuff> BuffList { get; set; } = [];
        public SceneBuff? TempBuff { get; set; }
        public bool IsAlive { get; private set; } = true;

        public int EventID { get; set; } = info.EventID;
        public int CustomStageID { get; set; } = 0;

        public void AddBuff(SceneBuff buff)
        {
            BuffList.Add(buff);
            Scene.Player.SendPacket(new PacketSyncEntityBuffChangeListScNotify(this, buff));
        }

        public void ApplyBuff(BattleInstance instance)
        {
            if (TempBuff != null)
            {
                instance.Buffs.Add(new MazeBuff(TempBuff));
                TempBuff = null;
            }
            foreach (var buff in BuffList)
            {
                if (buff.IsExpired())
                {
                    continue;
                }
                instance.Buffs.Add(new MazeBuff(buff));
            }
            Scene.Player.SendPacket(new PacketSyncEntityBuffChangeListScNotify(this, BuffList));

            BuffList.Clear();
        }

        public int GetStageId()
        {
            if (CustomStageID > 0)
            {
                return CustomStageID;
            }
            var id = Info.EventID * 10 + Scene.Player.Data.WorldLevel;
            if (GameData.StageConfigData.ContainsKey(id))
                return id;
            else
                return Info.EventID;
        }

        public List<ItemData> Kill(bool sendPacket = true)
        {
            Scene.RemoveEntity(this);
            IsAlive = false;

            GameData.MonsterDropData.TryGetValue(MonsterData.ID * 10 + Scene.Player.Data.WorldLevel, out var dropData);
            if (dropData == null) return [];
            var dropItems = dropData.CalculateDrop();
            Scene.Player.InventoryManager!.AddItems(dropItems, sendPacket);

            // TODO: Rogue support
            // call mission handler
            Scene.Player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.KillMonster, this);
            return dropItems;
        }

        public SceneEntityInfo ToProto()
        {
            return new()
            {
                EntityId = (uint)EntityID,
                GroupId = (uint)GroupID,
                InstId = (uint)InstID,
                Motion = new()
                {
                    Pos = Position.ToProto(),
                    Rot = Rotation.ToProto()
                },
                NpcMonster = new()
                {
                    EventId = (uint)EventID,
                    MonsterId = (uint)MonsterData.ID,
                    WorldLevel = (uint)Scene.Player.Data.WorldLevel,
                }
            };
        }
    }
}
