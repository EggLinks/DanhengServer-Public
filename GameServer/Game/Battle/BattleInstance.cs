using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;
using Google.Protobuf;

namespace EggLink.DanhengServer.Game.Battle
{
    public class BattleInstance(PlayerInstance player, Database.Lineup.LineupInfo lineup, List<StageConfigExcel> stages) : BasePlayerManager(player)
    {
        public int BattleId { get; set; } = ++player.NextBattleId;
        public int StaminaCost { get; set; }
        public int WorldLevel { get; set; }
        public int CocoonWave { get; set; }
        public int MappingInfoId { get; set; }
        public int RoundLimit { get; set; }
        public int StageId { get; set; } = stages.Count > 0 ? stages[0].StageID : 0;  // Set to 0 when hit monster
        public int EventId { get; set; }
        public int CustomLevel { get; set; }
        public BattleEndStatus BattleEndStatus { get; set; }

        public List<ItemData> MonsterDropItems { get; set; } = [];

        public List<StageConfigExcel> Stages { get; set; } = stages;
        public Database.Lineup.LineupInfo Lineup { get; set; } = lineup;
        public List<EntityMonster> EntityMonsters { get; set; } = [];
        public List<AvatarSceneInfo> AvatarInfo { get; set; } = [];
        public List<MazeBuff> Buffs { get; set; } = [];
        public Dictionary<int, BattleEventInstance> BattleEvents { get; set; } = [];
        public Dictionary<int, BattleTargetList> BattleTargets { get; set; } = [];

        public BattleInstance(PlayerInstance player, Database.Lineup.LineupInfo lineup, List<EntityMonster> monsters) : this(player, lineup, new List<StageConfigExcel>())
        {
            foreach (var monster in monsters)
            {
                var id = monster.GetStageId();
                GameData.StageConfigData.TryGetValue(id, out var stage);
                if (stage != null)
                {
                    Stages.Add(stage);
                }
            }
            EntityMonsters = monsters;
            StageId = Stages[0].StageID;
        }

        public ItemList GetDropItemList()
        {
            if (BattleEndStatus != BattleEndStatus.BattleEndWin) return new();
            var list = new ItemList();

            foreach (var item in MonsterDropItems)
            {
                list.ItemList_.Add(item.ToProto());
            }

            foreach (var item in Player.InventoryManager!.HandleMappingInfo(MappingInfoId, WorldLevel))
            {
                list.ItemList_.Add(item.ToProto());
            }

            return list;
        }

        public void AddBattleTarget(int key, int targetId, int progress, int totalProgress = 0)
        {
            if (!BattleTargets.TryGetValue(key, out BattleTargetList? value))
            {
                value = new BattleTargetList();
                BattleTargets.Add(key, value);
            }

            var battleTarget = new BattleTarget()
            {
                Id = (uint)targetId,
                Progress = (uint)progress,
                TotalProgress = (uint)totalProgress
            };
            value.BattleTargetList_.Add(battleTarget);
        }

        public Dictionary<AvatarInfo, AvatarType> GetBattleAvatars()
        {
            var excel = GameData.StageConfigData[StageId];
            List<int> list = [.. excel.TrialAvatarList];

            if (list.Count > 0)
            {
                if (Player.Data.CurrentGender == Gender.Man)
                {
                    foreach (var avatar in excel.TrialAvatarList)
                    {
                        if (avatar > 10000)  // else is Base Avatar
                        {
                            if (avatar.ToString().EndsWith("8002") ||
                                avatar.ToString().EndsWith("8004") ||
                                avatar.ToString().EndsWith("8006"))
                            {
                                list.Remove(avatar);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var avatar in excel.TrialAvatarList)
                    {
                        if (avatar > 10000)  // else is Base Avatar
                        {
                            if (avatar.ToString().EndsWith("8001") ||
                                avatar.ToString().EndsWith("8003") ||
                                avatar.ToString().EndsWith("8005"))
                            {
                                list.Remove(avatar);
                            }
                        }
                    }
                }
            }

            if (list.Count > 0)
            {
                Dictionary<AvatarInfo, AvatarType> dict = [];
                foreach (var avatar in list)
                {
                    GameData.SpecialAvatarData.TryGetValue(avatar * 10 + Player.Data.WorldLevel, out var specialAvatar);
                    if (specialAvatar != null)
                    {
                        dict.Add(specialAvatar.ToAvatarData(Player.Uid), AvatarType.AvatarTrialType);
                    } 
                    else
                    {
                        var avatarInfo = Player.AvatarManager!.GetAvatar(avatar);
                        if (avatarInfo != null)
                        {
                            dict.Add(avatarInfo, AvatarType.AvatarFormalType);
                        }
                    }
                }

                return dict;
            }
            else
            {
                Dictionary<AvatarInfo, AvatarType> dict = [];
                foreach (var avatar in Lineup.BaseAvatars!)
                {
                    AvatarInfo? avatarInstance = null;
                    AvatarType avatarType = AvatarType.AvatarFormalType;

                    if (avatar.AssistUid != 0)
                    {
                        var player = DatabaseHelper.Instance!.GetInstance<AvatarData>(avatar.AssistUid);
                        if (player != null)
                        {
                            avatarInstance = player.Avatars!.Find(item => item.GetAvatarId() == avatar.BaseAvatarId);
                            avatarType = AvatarType.AvatarAssistType;
                        }
                    }
                    else if (avatar.SpecialAvatarId != 0)
                    {
                        GameData.SpecialAvatarData.TryGetValue(avatar.SpecialAvatarId, out var specialAvatar);
                        if (specialAvatar != null)
                        {
                            avatarInstance = specialAvatar.ToAvatarData(Player.Uid);
                            avatarType = AvatarType.AvatarTrialType;
                        }
                    }
                    else
                    {
                        avatarInstance = Player.AvatarManager!.GetAvatar(avatar.BaseAvatarId);
                    }
                    if (avatarInstance == null) continue;

                    dict.Add(avatarInstance, avatarType);
                }

                return dict;
            }
        }

        public SceneBattleInfo ToProto()
        {
            var proto = new SceneBattleInfo()
            {
                BattleId = (uint)BattleId,
                WorldLevel = (uint)WorldLevel,
                RoundsLimit = (uint)RoundLimit,
                StageId = (uint)StageId,
                LogicRandomSeed = (uint)Random.Shared.Next(),
            };

            foreach (var wave in Stages)
            {
                var protoWave = wave.ToProto();
                if (CustomLevel > 0)
                {
                    foreach (var item in protoWave)
                    {
                        item.MonsterParam.Level = (uint)CustomLevel;
                    }
                }
                proto.MonsterWaveList.AddRange(protoWave);
            }

            foreach (var avatar in GetBattleAvatars())
            {
                proto.AvatarBattleList.Add(avatar.Key.ToBattleProto(Player.LineupManager!.GetCurLineup()!, Player.InventoryManager!.Data, avatar.Value));
            }

            foreach (var monster in EntityMonsters)
            {
                monster.ApplyBuff(this);
            }

            foreach (var avatar in AvatarInfo)
            {
                avatar.ApplyBuff(this);
            }

            foreach (var eventInstance in BattleEvents.Values)
            {
                proto.BattleEvent.Add(eventInstance.ToProto());
            }

            if (BattleTargets != null)
            {
                for (int i = 1; i <= 5; i++)
                {
                    var battleTargetEntry = new BattleTargetList();

                    if (BattleTargets.ContainsKey(i))
                    {
                        var battleTargetList = BattleTargets[i];
                        battleTargetEntry.BattleTargetList_.AddRange(battleTargetList.BattleTargetList_);
                    }

                    proto.BattleTargetInfo.Add((uint)i, battleTargetEntry);
                }
            }

            proto.BuffList.AddRange(Buffs.Select(buff => buff.ToProto(this)));
            return proto;
        }
    }
}
