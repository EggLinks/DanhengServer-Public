using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.Database.Scene;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game;
using EggLink.DanhengServer.Game.Lineup;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Raid
{
    public class RaidManager : BasePlayerManager
    {
        public RaidData RaidData { get; private set; }

        public RaidManager(PlayerInstance player) : base(player)
        {
            RaidData = DatabaseHelper.Instance!.GetInstanceOrCreateNew<RaidData>(player.Uid);
            OnLogin();
        }

        #region Player Action

        public void EnterRaid(int raidId, int worldLevel, List<int>? avatarList = null, bool enterSaved = false)
        {
            if (RaidData.CurRaidId != 0) return;

            GameData.RaidConfigData.TryGetValue(raidId * 100 + worldLevel, out var excel);
            if (excel == null) return;  // not exist

            RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict);
            dict ??= [];
            if (dict.ContainsKey(worldLevel) && !enterSaved)
            {
                // clear old record
                ClearRaid(raidId, worldLevel);
            }
            dict.TryGetValue(worldLevel, out var record);

            RaidData.CurRaidId = excel.RaidID;
            RaidData.CurRaidWorldLevel = worldLevel;

            if (record == null)
            {
                // first enter
                var entranceId = 0;
                var firstMission = excel.MainMissionIDList[0];
                var subMissionId = GameData.MainMissionData[firstMission].MissionInfo!.StartSubMissionList[0];  // get the first sub mission
                var subMission = GameData.SubMissionData[subMissionId];

                entranceId = int.Parse(subMission.SubMissionInfo!.LevelFloorID.ToString().Replace("00", "0"));  // get entrance id  ( need to find a better way to do it )

                if (!GameData.MapEntranceData.ContainsKey(entranceId))
                {
                    entranceId = subMission.SubMissionInfo!.LevelFloorID;
                }

                if (avatarList?.Count > 0)
                {
                    Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, avatarList);
                    Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
                }
                else if (excel.TeamType == Enums.Scene.RaidTeamTypeEnum.TrialOnly)
                {
                    // set lineup
                    List<int> list = [..excel.TrialAvatarList];
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
                    Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, list);
                    Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
                }
                else
                {
                    // set cur lineup
                    var lineup = Player.LineupManager!.GetCurLineup()!;
                    Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, lineup.BaseAvatars!.Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId).ToList());
                    Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
                }
                var oldEntryId = Player.Data.EntryId;
                var oldPos = Player.Data.Pos;
                var oldRot = Player.Data.Rot;

                Player.MissionManager!.AcceptMainMission(firstMission);

                Player.EnterScene(entranceId, 0, true);

                record = new RaidRecord()
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

                if (RaidData.RaidRecordDatas.TryGetValue(raidId, out Dictionary<int, RaidRecord>? value))
                {
                    value[worldLevel] = record;
                }
                else
                {
                    RaidData.RaidRecordDatas[raidId] = new Dictionary<int, RaidRecord>() { { worldLevel, record } };
                }
            }
            else
            {
                // just resume
                record.Status = RaidStatus.Doing;
                Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, record.Lineup.Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId).ToList());
                Player.LoadScene(record.PlaneId, record.FloorId, record.EntryId, record.Pos, record.Rot, true);
            }

            Player.SendPacket(new PacketRaidInfoNotify(record));
        }

        public void CheckIfLeaveRaid()
        {
            if (RaidData.CurRaidId == 0) return;

            var record = RaidData.RaidRecordDatas[RaidData.CurRaidId][RaidData.CurRaidWorldLevel];

            GameData.RaidConfigData.TryGetValue(RaidData.CurRaidId * 100 + record.WorldLevel, out var excel);
            if (excel == null) return;
            bool leave = true;
            foreach (var id in excel.MainMissionIDList)
            {
                if (Player.MissionManager!.GetMainMissionStatus(id) != MissionPhaseEnum.Finish)
                {
                    leave = false;
                }
            }

            if (leave)
            {
                FinishRaid();
                // finish
                Player.MissionManager!.HandleFinishType(MissionFinishTypeEnum.RaidFinishCnt);
            }
        }

        public void FinishRaid()
        {
            if (RaidData.CurRaidId == 0) return;

            var record = RaidData.RaidRecordDatas[RaidData.CurRaidId][RaidData.CurRaidWorldLevel];
            GameData.RaidConfigData.TryGetValue(RaidData.CurRaidId * 100 + record.WorldLevel, out var config);
            if (config == null) return;

            record.Status = RaidStatus.Finish;

            Player.SendPacket(new PacketRaidInfoNotify(record));
        }

        public void LeaveRaid(bool save)
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
                Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            }

            if (record.Status == RaidStatus.Finish)
            {
                if (config.FinishEntranceID > 0)
                {
                    Player.EnterScene(config.FinishEntranceID, 0, true);
                }
                else
                {
                    Player.EnterScene(record.OldEntryId, 0, true);
                    Player.MoveTo(record.OldPos, record.OldRot);
                }
            }
            else
            {
                Player.EnterScene(record.OldEntryId, 0, true);
                Player.MoveTo(record.OldPos, record.OldRot);

                // reset raid info
                record.Status = RaidStatus.Doing;

                Player.SendPacket(new PacketRaidInfoNotify(record));

                if (!save)
                {
                    ClearRaid(record.RaidId, record.WorldLevel);
                }
            }

            RaidData.CurRaidId = 0;
            RaidData.CurRaidWorldLevel = 0;
        }

        public void ClearRaid(int raidId, int worldLevel)
        {
            if (!RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict)) return;
            if (!dict.TryGetValue(worldLevel, out var record)) return;

            GameData.RaidConfigData.TryGetValue(raidId * 100 + worldLevel, out var config);
            if (config == null) return;

            config.MainMissionIDList.ForEach(missionId =>
            {
                Player.MissionManager!.RemoveMainMission(missionId);
            });

            dict.Remove(worldLevel);

            if (dict.Count == 0)
            {
                RaidData.RaidRecordDatas.Remove(raidId);
            }

            Player.SendPacket(new PacketDelSaveRaidScNotify(raidId, worldLevel));
        }

        #endregion

        #region Information

        public RaidStatus GetRaidStatus(int raidId, int worldLevel = 0)
        {
            if (!RaidData.RaidRecordDatas.TryGetValue(raidId, out var dict)) return RaidStatus.None;
            if (!dict.TryGetValue(worldLevel, out var record)) return RaidStatus.None;
            return record.Status;
        }

        #endregion

        #region Player Handler

        public void OnLogin()
        {
            // try resume
            if (RaidData.CurRaidId > 0 && RaidData.RaidRecordDatas.TryGetValue(RaidData.CurRaidId, out Dictionary<int, RaidRecord>? value))
            {
                if (value.TryGetValue(RaidData.CurRaidWorldLevel, out var record))
                {
                    Player.SendPacket(new PacketRaidInfoNotify(record));
                }
                else
                {
                    RaidData.CurRaidId = 0;
                    RaidData.CurRaidWorldLevel = 0;
                }
            } else
            {
                RaidData.CurRaidId = 0;
                RaidData.CurRaidWorldLevel = 0;
            }
        }

        #endregion
    }
}