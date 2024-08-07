using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;
using EggLink.DanhengServer.Proto;
using RaidData = EggLink.DanhengServer.Database.Scene.RaidData;

namespace EggLink.DanhengServer.GameServer.Game.Raid;

public class RaidManager : BasePlayerManager
{
    public RaidManager(PlayerInstance player) : base(player)
    {
        RaidData = DatabaseHelper.Instance!.GetInstanceOrCreateNew<RaidData>(player.Uid);
    }

    public RaidData RaidData { get; }

    #region Information

    public RaidStatus GetRaidStatus(int raidId, int worldLevel = 0)
    {
        if (!RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict)) return RaidStatus.None;
        if (!dict.TryGetValue(worldLevel, out var record)) return RaidStatus.None;
        return record.Status;
    }

    #endregion

    #region Player Handler

    public async ValueTask OnLogin()
    {
        // try resume
        if (RaidData.CurRaidId > 0 && RaidData.RaidRecordDatas.TryGetValue(RaidData.CurRaidId, out var value))
        {
            if (value.TryGetValue(RaidData.CurRaidWorldLevel, out var record))
            {
                await Player.SendPacket(new PacketRaidInfoNotify(record));
            }
            else
            {
                RaidData.CurRaidId = 0;
                RaidData.CurRaidWorldLevel = 0;
                await Player.SendPacket(new PacketRaidInfoNotify());
            }
        }
        else
        {
            RaidData.CurRaidId = 0;
            RaidData.CurRaidWorldLevel = 0;
            await Player.SendPacket(new PacketRaidInfoNotify());
        }
    }

    #endregion

    #region Player Action

    public async ValueTask EnterRaid(int raidId, int worldLevel, List<int>? avatarList = null, bool enterSaved = false)
    {
        if (RaidData.CurRaidId != 0) return;

        GameData.RaidConfigData.TryGetValue(raidId * 100 + worldLevel, out var excel);
        if (excel == null) return; // not exist

        RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict);
        dict ??= [];
        if (dict.ContainsKey(worldLevel) && !enterSaved)
            // clear old record
            await ClearRaid(raidId, worldLevel);
        dict.TryGetValue(worldLevel, out var record);

        RaidData.CurRaidId = excel.RaidID;
        RaidData.CurRaidWorldLevel = worldLevel;

        if (record == null)
        {
            // first enter
            var entranceId = 0;
            var firstMission = excel.MainMissionIDList[0];
            var subMissionId =
                GameData.MainMissionData[firstMission].MissionInfo!.StartSubMissionList[0]; // get the first sub mission
            var subMission = GameData.SubMissionData[subMissionId];

            entranceId =
                int.Parse(subMission.SubMissionInfo!.LevelFloorID.ToString()
                    .Replace("00", "0")); // get entrance id  ( need to find a better way to do it )

            if (!GameData.MapEntranceData.ContainsKey(entranceId)) entranceId = subMission.SubMissionInfo!.LevelFloorID;

            if (avatarList?.Count > 0)
            {
                Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, avatarList);
                await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            }
            else if (excel.TeamType == RaidTeamTypeEnum.TrialOnly)
            {
                // set lineup
                List<int> list = [..excel.TrialAvatarList];
                if (list.Count > 0)
                {
                    if (Player.Data.CurrentGender == Gender.Man)
                    {
                        foreach (var avatar in excel.TrialAvatarList)
                            if (avatar > 10000) // else is Base Avatar
                                if (avatar.ToString().EndsWith("8002") ||
                                    avatar.ToString().EndsWith("8004") ||
                                    avatar.ToString().EndsWith("8006"))
                                    list.Remove(avatar);
                    }
                    else
                    {
                        foreach (var avatar in excel.TrialAvatarList)
                            if (avatar > 10000) // else is Base Avatar
                                if (avatar.ToString().EndsWith("8001") ||
                                    avatar.ToString().EndsWith("8003") ||
                                    avatar.ToString().EndsWith("8005"))
                                    list.Remove(avatar);
                    }
                }

                Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, list);
                if (excel.LockCaptain) Player.LineupManager!.GetCurLineup()!.LeaderAvatarId = excel.LockCaptainAvatarID;
                await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            }
            else
            {
                // set cur lineup
                var lineup = Player.LineupManager!.GetCurLineup()!;
                Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus,
                    lineup.BaseAvatars!.Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId)
                        .ToList());
                await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            }

            var oldEntryId = Player.Data.EntryId;
            var oldPos = Player.Data.Pos;
            var oldRot = Player.Data.Rot;

            await Player.MissionManager!.AcceptMainMission(firstMission);

            await Player.EnterScene(entranceId, 0, true);

            record = new RaidRecord
            {
                PlaneId = Player.Data.PlaneId,
                FloorId = Player.Data.FloorId,
                EntryId = entranceId,
                Pos = Player.Data.Pos!,
                Rot = Player.Data.Rot!,
                Status = RaidStatus.Doing,
                WorldLevel = worldLevel,
                RaidId = raidId,
                Lineup = Player.LineupManager!.GetCurLineup()!.BaseAvatars!,
                OldEntryId = oldEntryId,
                OldPos = oldPos!,
                OldRot = oldRot!
            };

            if (RaidData.RaidRecordDatas.TryGetValue(raidId, out var value))
                value[worldLevel] = record;
            else
                RaidData.RaidRecordDatas[raidId] = new Dictionary<int, RaidRecord> { { worldLevel, record } };
        }
        else
        {
            // just resume
            record.Status = RaidStatus.Doing;
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus,
                record.Lineup.Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId).ToList());
            await Player.LoadScene(record.PlaneId, record.FloorId, record.EntryId, record.Pos, record.Rot, true);
        }

        await Player.SendPacket(new PacketRaidInfoNotify(record));
    }

    public async ValueTask CheckIfLeaveRaid()
    {
        if (RaidData.CurRaidId == 0) return;

        var record = RaidData.RaidRecordDatas[RaidData.CurRaidId][RaidData.CurRaidWorldLevel];

        GameData.RaidConfigData.TryGetValue(RaidData.CurRaidId * 100 + record.WorldLevel, out var excel);
        if (excel == null) return;
        var leave = true;
        foreach (var id in excel.MainMissionIDList)
            if (Player.MissionManager!.GetMainMissionStatus(id) != MissionPhaseEnum.Finish)
                leave = false;

        if (leave)
        {
            await FinishRaid();
            // finish
            await Player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.RaidFinishCnt);
        }
    }

    public async ValueTask FinishRaid()
    {
        if (RaidData.CurRaidId == 0) return;

        var record = RaidData.RaidRecordDatas[RaidData.CurRaidId][RaidData.CurRaidWorldLevel];
        GameData.RaidConfigData.TryGetValue(RaidData.CurRaidId * 100 + record.WorldLevel, out var config);
        if (config == null) return;

        record.Status = RaidStatus.Finish;

        await Player.SendPacket(new PacketRaidInfoNotify(record));
    }

    public async ValueTask LeaveRaid(bool save)
    {
        if (RaidData.CurRaidId == 0) return;

        var record = RaidData.RaidRecordDatas[RaidData.CurRaidId][RaidData.CurRaidWorldLevel];
        GameData.RaidConfigData.TryGetValue(RaidData.CurRaidId * 100 + record.WorldLevel, out var config);
        if (config == null) return;

        record.PlaneId = Player.Data.PlaneId;
        record.FloorId = Player.Data.FloorId;
        record.EntryId = Player.Data.EntryId;
        record.Pos = Player.Data.Pos!;
        record.Rot = Player.Data.Rot!;

        if (Player.LineupManager!.GetCurLineup()!.IsExtraLineup())
        {
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);
            await Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
        }

        if (record.Status == RaidStatus.Finish)
        {
            await Player.SendPacket(new PacketRaidInfoNotify());
            if (config.FinishEntranceID > 0)
            {
                await Player.EnterScene(config.FinishEntranceID, 0, true);
            }
            else
            {
                await Player.EnterScene(record.OldEntryId, 0, true);
                await Player.MoveTo(record.OldPos, record.OldRot);
            }
        }
        else
        {
            await Player.EnterScene(record.OldEntryId, 0, true);
            await Player.MoveTo(record.OldPos, record.OldRot);

            // reset raid info

            await Player.SendPacket(new PacketRaidInfoNotify());

            if (!save) await ClearRaid(record.RaidId, record.WorldLevel);
        }

        RaidData.CurRaidId = 0;
        RaidData.CurRaidWorldLevel = 0;
    }

    public async ValueTask ClearRaid(int raidId, int worldLevel)
    {
        if (!RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict)) return;
        if (!dict.TryGetValue(worldLevel, out var record)) return;

        GameData.RaidConfigData.TryGetValue(raidId * 100 + worldLevel, out var config);
        if (config == null) return;

        config.MainMissionIDList.ForEach(async missionId =>
        {
            await Player.MissionManager!.RemoveMainMission(missionId);
        });

        dict.Remove(worldLevel);

        if (dict.Count == 0) RaidData.RaidRecordDatas.Remove(raidId);

        await Player.SendPacket(new PacketDelSaveRaidScNotify(raidId, worldLevel));
    }

    #endregion
}