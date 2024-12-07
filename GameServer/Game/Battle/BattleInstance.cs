using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Avatar;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.BattleCollege;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using LineupInfo = EggLink.DanhengServer.Database.Lineup.LineupInfo;

namespace EggLink.DanhengServer.GameServer.Game.Battle;

public class BattleInstance(PlayerInstance player, LineupInfo lineup, List<StageConfigExcel> stages)
    : BasePlayerManager(player)
{
    public BattleInstance(PlayerInstance player, LineupInfo lineup, List<EntityMonster> monsters) : this(player, lineup,
        new List<StageConfigExcel>())
    {
        if (player.ActivityManager!.TrialActivityInstance != null &&
            player.ActivityManager!.TrialActivityInstance.Data.CurTrialStageId != 0)
        {
            var instance = player.ActivityManager!.TrialActivityInstance;
            GameData.StageConfigData.TryGetValue(instance.Data.CurTrialStageId, out var stage);
            if (stage != null) Stages.Add(stage);
            StageId = Stages[0].StageID;
        }
        else
        {
            foreach (var id in monsters.Select(monster => monster.GetStageId()))
            {
                GameData.PlaneEventData.TryGetValue(id * 10 + player.Data.WorldLevel, out var planeEvent);
                if (planeEvent == null) continue;
                GameData.StageConfigData.TryGetValue(planeEvent.StageID, out var stage);
                if (stage != null) Stages.Add(stage);
            }

            EntityMonsters = monsters;
            StageId = Stages[0].StageID;
        }
    }

    public int BattleId { get; set; } = ++player.NextBattleId;
    public int StaminaCost { get; set; }
    public int WorldLevel { get; set; }
    public int CocoonWave { get; set; }
    public int MappingInfoId { get; set; }
    public int RoundLimit { get; set; }
    public int StageId { get; set; } = stages.Count > 0 ? stages[0].StageID : 0; // Set to 0 when hit monster
    public int EventId { get; set; }
    public int CustomLevel { get; set; }
    public BattleEndStatus BattleEndStatus { get; set; }

    public List<ItemData> MonsterDropItems { get; set; } = [];

    public List<StageConfigExcel> Stages { get; set; } = stages;
    public LineupInfo Lineup { get; set; } = lineup;
    public List<EntityMonster> EntityMonsters { get; set; } = [];
    public List<AvatarSceneInfo> AvatarInfo { get; set; } = [];
    public List<MazeBuff> Buffs { get; set; } = [];
    public BattleRogueMagicInfo? MagicInfo { get; set; }
    public Dictionary<int, BattleEventInstance> BattleEvents { get; set; } = [];
    public Dictionary<int, BattleTargetList> BattleTargets { get; set; } = [];
    public BattleCollegeConfigExcel? CollegeConfigExcel { get; set; }
    public PVEBattleResultCsReq? BattleResult { get; set; }

    public ItemList GetDropItemList()
    {
        if (BattleEndStatus != BattleEndStatus.BattleEndWin) return new ItemList();
        var list = new ItemList();

        foreach (var item in MonsterDropItems) list.ItemList_.Add(item.ToProto());

        var t = System.Threading.Tasks.Task.Run(async () =>
        {
            foreach (var item in await Player.InventoryManager!.HandleMappingInfo(MappingInfoId, WorldLevel))
                list.ItemList_.Add(item.ToProto());
        });

        t.Wait();

        if (CollegeConfigExcel == null ||
            Player.BattleCollegeData?.FinishedCollegeIdList.Contains(CollegeConfigExcel.ID) != false)
            return list; // if college excel is not null and college is not finished

        // finish it 
        Player.BattleCollegeData.FinishedCollegeIdList.Add(CollegeConfigExcel.ID);
        var t2 = System.Threading.Tasks.Task.Run(async () =>
        {
            await Player.SendPacket(new PacketBattleCollegeDataChangeScNotify(Player));
            foreach (var item in await Player.InventoryManager!.HandleReward(CollegeConfigExcel.RewardID))
                list.ItemList_.Add(item.ToProto());
        });

        t2.Wait();

        return list;
    }

    public void AddBattleTarget(int key, int targetId, int progress, int totalProgress = 0)
    {
        if (!BattleTargets.TryGetValue(key, out var value))
        {
            value = new BattleTargetList();
            BattleTargets.Add(key, value);
        }

        var battleTarget = new BattleTarget
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

        // if college excel is not null
        if (CollegeConfigExcel is { TrialAvatarList.Count: > 0 }) list = [.. CollegeConfigExcel.TrialAvatarList];

        if (list.Count > 0)
        {
            List<int> tempList = [.. list];
            if (Player.Data.CurrentGender == Gender.Man)
                foreach (var avatar in tempList.Where(avatar =>
                             GameData.SpecialAvatarData.TryGetValue(avatar * 10 + 0, out var specialAvatarExcel) &&
                             specialAvatarExcel.AvatarID is 8002 or 8004 or 8006))
                    list.Remove(avatar);
            else
                foreach (var avatar in tempList.Where(avatar =>
                             GameData.SpecialAvatarData.TryGetValue(avatar * 10 + 0, out var specialAvatarExcel) &&
                             specialAvatarExcel.AvatarID is 8001 or 8003 or 8005))
                    list.Remove(avatar);
        }

        if (list.Count > 0) // if list is not empty
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
                    GameData.SpecialAvatarData.TryGetValue(avatar * 10 + 0, out var raw);
                    if (raw != null)
                    {
                        dict.Add(raw.ToAvatarData(Player.Uid), AvatarType.AvatarTrialType);
                    }
                    else
                    {
                        var avatarInfo = Player.AvatarManager!.GetAvatar(avatar);
                        if (avatarInfo != null) dict.Add(avatarInfo, AvatarType.AvatarFormalType);
                    }
                }
            }

            return dict;
        }
        else
        {
            Dictionary<AvatarInfo, AvatarType> dict = [];
            foreach (var avatar in Lineup.BaseAvatars!) // if list is empty, use scene lineup
            {
                AvatarInfo? avatarInstance = null;
                var avatarType = AvatarType.AvatarFormalType;

                if (avatar.AssistUid != 0)
                {
                    var player = DatabaseHelper.Instance!.GetInstance<AvatarData>(avatar.AssistUid);
                    if (player != null)
                    {
                        avatarInstance = player.Avatars.Find(item => item.GetAvatarId() == avatar.BaseAvatarId);
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
        var proto = new SceneBattleInfo
        {
            BattleId = (uint)BattleId,
            WorldLevel = (uint)WorldLevel,
            RoundsLimit = (uint)RoundLimit,
            StageId = (uint)StageId,
            LogicRandomSeed = (uint)Random.Shared.Next()
        };

        if (MagicInfo != null) proto.BattleRogueMagicInfo = MagicInfo;

        foreach (var protoWave in Stages.Select(wave => wave.ToProto()))
        {
            if (CustomLevel > 0)
                foreach (var item in protoWave)
                    item.MonsterParam.Level = (uint)CustomLevel;
            proto.MonsterWaveList.AddRange(protoWave);
        }

        var avatars = GetBattleAvatars();
        foreach (var avatar in avatars)
            proto.BattleAvatarList.Add(avatar.Key.ToBattleProto(Player.LineupManager!.GetCurLineup()!,
                Player.InventoryManager!.Data, avatar.Value));

        System.Threading.Tasks.Task.Run(async () =>
        {
            foreach (var monster in EntityMonsters) await monster.ApplyBuff(this);

            foreach (var avatar in AvatarInfo)
                if (avatars.Keys.FirstOrDefault(x =>
                        x.GetSpecialAvatarId() == avatar.AvatarInfo.GetSpecialAvatarId()) !=
                    null) // if avatar is in lineup
                    await avatar.ApplyBuff(this);
        }).Wait();

        foreach (var eventInstance in BattleEvents.Values) proto.BattleEvent.Add(eventInstance.ToProto());

        for (var i = 1; i <= 5; i++)
        {
            var battleTargetEntry = new BattleTargetList();

            if (BattleTargets.TryGetValue(i, out var battleTargetList))
                battleTargetEntry.BattleTargetList_.AddRange(battleTargetList.BattleTargetList_);

            proto.BattleTargetInfo.Add((uint)i, battleTargetEntry);
        }

        foreach (var buff in Buffs)
        {
            if (buff.WaveFlag != null) continue;
            var buffs = Buffs.FindAll(x => x.BuffID == buff.BuffID);
            if (buffs.Count < 2) continue;
            var count = 0;
            foreach (var mazeBuff in buffs)
            {
                mazeBuff.WaveFlag = (int)Math.Pow(2, count);
                count++;
            }
        }

        foreach (var buff in Buffs.Clone())
            if (buff.BuffID == 122003) // Fei Xiao Maze Buff
                Buffs.Add(new MazeBuff(122002, buff.BuffLevel, 0)
                {
                    WaveFlag = buff.WaveFlag,
                    OwnerAvatarId = buff.OwnerAvatarId
                });

        proto.BuffList.AddRange(Buffs.Select(buff => buff.ToProto(this)));
        return proto;
    }
}