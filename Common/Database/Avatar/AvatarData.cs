using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;
using SqlSugar;
using LineupInfo = EggLink.DanhengServer.Database.Lineup.LineupInfo;

namespace EggLink.DanhengServer.Database.Avatar;

[SugarTable("Avatar")]
public class AvatarData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public List<AvatarInfo> Avatars { get; set; } = [];

    [SugarColumn(IsJson = true)] public List<int> AssistAvatars { get; set; } = [];

    [SugarColumn(IsJson = true)] public List<int> DisplayAvatars { get; set; } = [];
}

public class AvatarInfo
{
    [JsonIgnore] public AvatarConfigExcel? Excel;

    [JsonIgnore] public PlayerData? PlayerData;

    public AvatarInfo()
    {
        // only for db
    }

    public AvatarInfo(AvatarConfigExcel excel)
    {
        Excel = excel;
        SkillTree = [];
        if (AvatarId == 8001)
        {
        }
        else
        {
            excel.DefaultSkillTree.ForEach(skill => { SkillTree.Add(skill.PointID, skill.Level); });
        }
    }

    public int AvatarId { get; set; }

    [JsonIgnore] public int SpecialBaseAvatarId { get; set; }

    public int PathId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Promotion { get; set; }
    public int Rewards { get; set; }
    public long Timestamp { get; set; }
    public int CurrentHp { get; set; } = 10000;
    public int CurrentSp { get; set; }
    public int ExtraLineupHp { get; set; } = 10000;
    public int ExtraLineupSp { get; set; }
    public bool IsMarked { get; set; } = false;
    public Dictionary<int, int> SkillTree { get; set; } = [];

    public Dictionary<int, Dictionary<int, int>> SkillTreeExtra { get; set; } =
        []; // for hero  heroId -> skillId -> level

    public Dictionary<int, PathInfo> PathInfoes { get; set; } = [];

    [JsonIgnore] public int InternalEntityId { get; set; }

    [JsonIgnore]
    public int EntityId
    {
        get => InternalEntityId;
        set
        {
            if (SpecialBaseAvatarId > 0 && PlayerData != null)
            {
                // set in SpecialAvatarExcel
                GameData.SpecialAvatarData.TryGetValue(SpecialBaseAvatarId * 10 + PlayerData.WorldLevel,
                    out var specialAvatar);
                if (specialAvatar != null)
                {
                    specialAvatar.EntityId[PlayerData.Uid] = value;
                    InternalEntityId = value;
                }
            }

            InternalEntityId = value;
        }
    }

    public void ValidateHero()
    {
        if (PathId == 0) return;

        var isWoman = PathId % 2 == 0;

        var shouldRemove = new List<int>();
        foreach (var skill in SkillTreeExtra.Keys)
            if (skill % 2 == 0 != isWoman) // remove
                shouldRemove.Add(skill);

        foreach (var skill in shouldRemove) SkillTreeExtra.Remove(skill);

        foreach (var path in PathInfoes.Keys)
            if (path % 2 == 0 != isWoman) // remove
                PathInfoes.Remove(path);
    }

    public bool HasTakenReward(int promotion)
    {
        return (Rewards & (1 << promotion)) != 0;
    }

    public void TakeReward(int promotion)
    {
        Rewards |= 1 << promotion;
    }

    public int GetCurHp(bool isExtraLineup)
    {
        return isExtraLineup ? ExtraLineupHp : CurrentHp;
    }

    public int GetCurSp(bool isExtraLineup)
    {
        return isExtraLineup ? ExtraLineupSp : CurrentSp;
    }

    public int GetAvatarId()
    {
        return PathId > 0 ? PathId : AvatarId;
    }

    public int GetBaseAvatarId()
    {
        if (PathId > 0)
            return PathId > 8000 ? 8001 : AvatarId;
        return AvatarId;
    }

    public int GetSpecialAvatarId()
    {
        return SpecialBaseAvatarId > 0 ? SpecialBaseAvatarId : GetAvatarId();
    }

    public int GetUniqueAvatarId()
    {
        return SpecialBaseAvatarId > 0 ? SpecialBaseAvatarId : GetBaseAvatarId();
    }

    public PathInfo GetCurPathInfo()
    {
        if (PathInfoes.ContainsKey(GetAvatarId())) return PathInfoes[GetAvatarId()];

        PathInfoes.Add(GetAvatarId(), new PathInfo(PathId));
        return PathInfoes[GetAvatarId()];
    }

    public PathInfo? GetPathInfo(int pathId)
    {
        if (PathInfoes.TryGetValue(pathId, out var value)) return value;
        return null;
    }

    public Dictionary<int, int> GetSkillTree(int pathId = 0)
    {
        if (pathId == 0) pathId = PathId;

        if (pathId == 0 && AvatarId == 1001)
        {
            PathId = 1001;
            pathId = 1001; // march 7th
        }

        var value = SkillTree;
        if (pathId > 0)
            if (!SkillTreeExtra.TryGetValue(pathId, out value))
            {
                value = [];
                // for old data
                SkillTreeExtra[pathId] = value;
                var excel = GameData.AvatarConfigData[pathId];
                excel.DefaultSkillTree.ForEach(skill => { SkillTreeExtra[pathId].Add(skill.PointID, skill.Level); });
            }

        return value;
    }

    public void VaildateSkillTree()
    {
        if (AvatarId < 8000)
            foreach (var avatar in GameData.MultiplePathAvatarConfigData.Values)
                if (avatar.AvatarID == AvatarId)
                    if (!SkillTreeExtra.TryGetValue(avatar.AvatarID, out var value))
                    {
                        value = [];
                        // for old data
                        SkillTreeExtra[avatar.AvatarID] = value;
                        var excel = GameData.AvatarConfigData[avatar.AvatarID];
                        excel.DefaultSkillTree.ForEach(skill =>
                        {
                            SkillTreeExtra[avatar.AvatarID].Add(skill.PointID, skill.Level);
                        });
                    }
    }

    public void SetCurHp(int value, bool isExtraLineup)
    {
        if (isExtraLineup)
            ExtraLineupHp = value;
        else
            CurrentHp = value;
    }

    public void SetCurSp(int value, bool isExtraLineup)
    {
        if (isExtraLineup)
            ExtraLineupSp = value;
        else
            CurrentSp = value;
    }

    public Proto.Avatar ToProto()
    {
        var proto = new Proto.Avatar
        {
            BaseAvatarId = (uint)GetUniqueAvatarId(),
            Level = (uint)Level,
            Exp = (uint)Exp,
            Promotion = (uint)Promotion,
            Rank = (uint)GetCurPathInfo().Rank,
            FirstMetTimeStamp = (ulong)Timestamp,
            IsMarked = IsMarked
        };

        foreach (var item in GetCurPathInfo().Relic)
            proto.EquipRelicList.Add(new EquipRelic
            {
                RelicUniqueId = (uint)item.Value,
                Type = (uint)item.Key
            });

        if (GetCurPathInfo().EquipId != 0) proto.EquipmentUniqueId = (uint)GetCurPathInfo().EquipId;

        foreach (var skill in GetSkillTree())
            proto.SkilltreeList.Add(new AvatarSkillTree
            {
                PointId = (uint)skill.Key,
                Level = (uint)skill.Value
            });

        for (var i = 0; i < Promotion; i++)
            if (HasTakenReward(i))
                proto.HasTakenPromotionRewardList.Add((uint)i);

        return proto;
    }

    public SceneEntityInfo ToSceneEntityInfo(AvatarType avatarType = AvatarType.AvatarFormalType)
    {
        return new SceneEntityInfo
        {
            EntityId = (uint)EntityId,
            Motion = new MotionInfo
            {
                Pos = PlayerData?.Pos?.ToProto() ?? new Vector(),
                Rot = PlayerData?.Rot?.ToProto() ?? new Vector()
            },
            Actor = new SceneActorInfo
            {
                BaseAvatarId = (uint)GetBaseAvatarId(),
                AvatarType = avatarType
            }
        };
    }

    public LineupAvatar ToLineupInfo(int slot, LineupInfo info, AvatarType avatarType = AvatarType.AvatarFormalType)
    {
        return new LineupAvatar
        {
            Id = (uint)GetUniqueAvatarId(),
            Slot = (uint)slot,
            AvatarType = avatarType,
            Hp = info.IsExtraLineup() ? (uint)ExtraLineupHp : (uint)CurrentHp,
            SpBar = new SpBarInfo
            {
                CurSp = info.IsExtraLineup() ? (uint)ExtraLineupSp : (uint)CurrentSp,
                MaxSp = 10000
            }
        };
    }

    public BattleAvatar ToBattleProto(LineupInfo lineup, InventoryData inventory,
        AvatarType avatarType = AvatarType.AvatarFormalType)
    {
        var proto = new BattleAvatar
        {
            Id = (uint)GetSpecialAvatarId(),
            AvatarType = avatarType,
            Level = (uint)Level,
            Promotion = (uint)Promotion,
            Rank = (uint)GetCurPathInfo().Rank,
            Index = (uint)lineup.GetSlot(GetBaseAvatarId()),
            Hp = (uint)GetCurHp(lineup.LineupType != 0),
            SpBar = new SpBarInfo
            {
                CurSp = (uint)GetCurSp(lineup.LineupType != 0),
                MaxSp = 10000
            },
            WorldLevel = (uint)(PlayerData?.WorldLevel ?? 0)
        };

        foreach (var skill in GetSkillTree())
            proto.SkilltreeList.Add(new AvatarSkillTree
            {
                PointId = (uint)skill.Key,
                Level = (uint)skill.Value
            });

        foreach (var relic in GetCurPathInfo().Relic)
        {
            var item = inventory.RelicItems?.Find(item => item.UniqueId == relic.Value);
            if (item != null)
            {
                var protoRelic = new BattleRelic
                {
                    Id = (uint)item.ItemId,
                    UniqueId = (uint)item.UniqueId,
                    Level = (uint)item.Level,
                    MainAffixId = (uint)item.MainAffix
                };

                if (item.SubAffixes.Count >= 1)
                    foreach (var subAffix in item.SubAffixes)
                        protoRelic.SubAffixList.Add(subAffix.ToProto());

                proto.RelicList.Add(protoRelic);
            }
        }

        if (GetCurPathInfo().EquipId != 0)
        {
            var item = inventory.EquipmentItems?.Find(item => item.UniqueId == GetCurPathInfo().EquipId);
            if (item != null)
                proto.EquipmentList.Add(new BattleEquipment
                {
                    Id = (uint)item.ItemId,
                    Level = (uint)item.Level,
                    Promotion = (uint)item.Promotion,
                    Rank = (uint)item.Rank
                });
        }
        else if (GetCurPathInfo().EquipData != null)
        {
            proto.EquipmentList.Add(new BattleEquipment
            {
                Id = (uint)GetCurPathInfo().EquipData!.ItemId,
                Level = (uint)GetCurPathInfo().EquipData!.Level,
                Promotion = (uint)GetCurPathInfo().EquipData!.Promotion,
                Rank = (uint)GetCurPathInfo().EquipData!.Rank
            });
        }

        return proto;
    }

    public List<MultiPathAvatarInfo> ToAvatarPathProto()
    {
        var res = new List<MultiPathAvatarInfo>();

        GetSkillTree();
        VaildateSkillTree();

        foreach (var path in SkillTreeExtra)
        {
            PathInfoes.TryGetValue(path.Key, out var pathInfo);

            if (pathInfo == null)
            {
                PathInfoes.Add(path.Key, new PathInfo(path.Key));
                pathInfo = PathInfoes[path.Key];
            }

            var proto = new MultiPathAvatarInfo
            {
                AvatarId = (MultiPathAvatarType)path.Key,
                Rank = (uint)GetCurPathInfo().Rank,
                PathEquipmentId = (uint)pathInfo.EquipId
            };

            foreach (var skill in path.Value)
                proto.MultiPathSkillTree.Add(new AvatarSkillTree
                {
                    PointId = (uint)skill.Key,
                    Level = (uint)skill.Value
                });

            foreach (var relic in pathInfo.Relic)
                proto.EquipRelicList.Add(new EquipRelic
                {
                    Type = (uint)relic.Key,
                    RelicUniqueId = (uint)relic.Value
                });

            res.Add(proto);
        }

        return res;
    }

    public DisplayAvatarDetailInfo ToDetailProto(int pos)
    {
        var proto = new DisplayAvatarDetailInfo
        {
            AvatarId = (uint)GetAvatarId(),
            Level = (uint)Level,
            Exp = (uint)Exp,
            Promotion = (uint)Promotion,
            Rank = (uint)GetCurPathInfo().Rank,
            Pos = (uint)pos
        };

        var inventory = DatabaseHelper.Instance!.GetInstance<InventoryData>(PlayerData!.Uid)!;
        foreach (var item in GetCurPathInfo().Relic)
        {
            var relic = inventory.RelicItems.Find(x => x.UniqueId == item.Value)!;
            proto.RelicList.Add(relic.ToDisplayRelicProto());
        }

        if (GetCurPathInfo().EquipId != 0)
        {
            var equip = inventory.EquipmentItems.Find(x => x.UniqueId == GetCurPathInfo().EquipId)!;
            proto.Equipment = equip.ToDisplayEquipmentProto();
        }

        foreach (var skill in GetSkillTree())
            proto.SkilltreeList.Add(new AvatarSkillTree
            {
                PointId = (uint)skill.Key,
                Level = (uint)skill.Value
            });

        return proto;
    }
}

public class PathInfo(int pathId)
{
    public int PathId { get; set; } = pathId;
    public int Rank { get; set; }
    public int EquipId { get; set; } = 0;
    public Dictionary<int, int> Relic { get; set; } = [];
    public ItemData? EquipData { get; set; } // for special avatar
}