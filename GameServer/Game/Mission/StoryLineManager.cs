using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Mission;
using EggLink.DanhengServer.Game;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Game.Mission
{
    public class StoryLineManager : BasePlayerManager
    {
        public StoryLineData StoryLineData { get; set; }

        public StoryLineManager(PlayerInstance player) : base(player)
        {
            StoryLineData = DatabaseHelper.Instance!.GetInstanceOrCreateNew<StoryLineData>(player.Uid);
        }

        public void CheckIfEnterStoryLine()
        {
            if (StoryLineData.CurStoryLineId != 0) return;

            foreach (var storyLine in GameData.StoryLineData.Values)
            {
                if (Player.MissionManager!.GetSubMissionStatus(storyLine.BeginCondition.Param) == Enums.MissionPhaseEnum.Finish)
                {
                    InitStoryLine(storyLine.StoryLineID);
                    return;
                }
            }
        }

        public void InitStoryLine(int storyLineId, int entryId = 0, int anchorGroupId = 0, int anchorId = 0)
        {
            if (StoryLineData.CurStoryLineId != 0)
            {
                FinishStoryLine(entryId, anchorGroupId, anchorId, false);
            }
            GameData.StoryLineData.TryGetValue(storyLineId, out var storyExcel);
            GameData.StroyLineTrialAvatarDataData.TryGetValue(storyLineId, out var storyAvatarExcel);
            if (storyExcel == null || storyAvatarExcel == null) return;
            StoryLineData.OldEntryId = Player.Data.EntryId;
            StoryLineData.OldFloorId = Player.Data.FloorId;
            StoryLineData.OldPlaneId = Player.Data.PlaneId;
            StoryLineData.OldPos = Player.Data.Pos!;
            StoryLineData.OldRot = Player.Data.Rot!;

            List<int> avatarList = Player.LineupManager!.GetCurLineup()!.BaseAvatars!.Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId).ToList();

            for (int i = 0; i < storyAvatarExcel.InitTrialAvatarList.Count; i++)
            {
                avatarList[i] = storyAvatarExcel.InitTrialAvatarList[i];  // replace the avatar with the special avatar
            }

            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, avatarList);

            StoryLineData.CurStoryLineId = storyExcel.StoryLineID;
            Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            Player.SendPacket(new PacketStoryLineInfoScNotify(Player));
            Player.SendPacket(new PacketChangeStoryLineFinishScNotify(storyExcel.StoryLineID, ChangeStoryLineAction.FinishAction));

            if (entryId > 0)
            {
                Player.EnterMissionScene(entryId, anchorGroupId, anchorId, true, ChangeStoryLineAction.FinishAction);
            }
            else
            {
                Player.EnterMissionScene(storyExcel.InitEntranceID, storyExcel.InitGroupID, storyExcel.InitAnchorID, true, ChangeStoryLineAction.FinishAction);
            }

            var record = new StoryLineInfo()
            {
                Lineup = Player.LineupManager!.GetCurLineup()!.BaseAvatars!,
                SavedEntryId = Player.Data.EntryId,
                SavedFloorId = Player.Data.FloorId,
                SavedPlaneId = Player.Data.PlaneId,
                SavedPos = Player.Data.Pos!,
                SavedRot = Player.Data.Rot!,
                StoryLineId = storyExcel.StoryLineID
            };

            StoryLineData.RunningStoryLines[storyExcel.StoryLineID] = record;
        }

        public void EnterStoryLine(int storyLineId, bool tp = true)
        {
            if (StoryLineData.CurStoryLineId == storyLineId) return;  // already in this story line

            if (storyLineId == 0)  // leave story line
            {
                LeaveStoryLine(tp);
                return;
            }

            StoryLineData.RunningStoryLines.TryGetValue(storyLineId, out var lineInfo);
            if (lineInfo == null) return;

            if (StoryLineData.CurStoryLineId == 0)  // not in any story line
            {
                StoryLineData.OldEntryId = Player.Data.EntryId;
                StoryLineData.OldFloorId = Player.Data.FloorId;
                StoryLineData.OldPlaneId = Player.Data.PlaneId;
                StoryLineData.OldPos = Player.Data.Pos!;
                StoryLineData.OldRot = Player.Data.Rot!;
            } 
            else  // in another story line
            {
                LeaveStoryLine(false);
            }

            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupHeliobus, lineInfo.Lineup.Select(x => x.SpecialAvatarId > 0 ? x.SpecialAvatarId / 10 : x.BaseAvatarId).ToList());
            
            StoryLineData.CurStoryLineId = lineInfo.StoryLineId;
            Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            Player.SendPacket(new PacketStoryLineInfoScNotify(Player));
            Player.SendPacket(new PacketChangeStoryLineFinishScNotify(StoryLineData.CurStoryLineId, ChangeStoryLineAction.Client));

            if (tp)
            {
                Player.LoadScene(lineInfo.SavedPlaneId, lineInfo.SavedFloorId, lineInfo.SavedEntryId, lineInfo.SavedPos, lineInfo.SavedRot, true, ChangeStoryLineAction.Client);
            }
        }

        public void LeaveStoryLine(bool tp)
        {
            if (StoryLineData.CurStoryLineId == 0) return;

            GameData.StoryLineData.TryGetValue(StoryLineData.CurStoryLineId, out var storyExcel);
            if (storyExcel == null) return;

            var record = new StoryLineInfo()
            {
                Lineup = Player.LineupManager!.GetCurLineup()!.BaseAvatars!,
                SavedEntryId = Player.Data.EntryId,
                SavedFloorId = Player.Data.FloorId,
                SavedPlaneId = Player.Data.PlaneId,
                SavedPos = Player.Data.Pos!,
                SavedRot = Player.Data.Rot!,
                StoryLineId = storyExcel.StoryLineID
            };

            // reset
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);

            // save
            StoryLineData.RunningStoryLines[storyExcel.StoryLineID] = record;
            StoryLineData.CurStoryLineId = 0;

            Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            Player.SendPacket(new PacketStoryLineInfoScNotify(Player));
            Player.SendPacket(new PacketChangeStoryLineFinishScNotify(0, ChangeStoryLineAction.None));

            if (tp)
            {
                Player.LoadScene(StoryLineData.OldPlaneId, StoryLineData.OldFloorId, StoryLineData.OldEntryId, StoryLineData.OldPos, StoryLineData.OldRot, true, ChangeStoryLineAction.None);

                StoryLineData.OldPlaneId = 0;
                StoryLineData.OldEntryId = 0;
                StoryLineData.OldFloorId = 0;
                StoryLineData.OldPos = new();
                StoryLineData.OldRot = new();
            }
        }

        public void CheckIfFinishStoryLine()  // seems like a story line end with another ChangeStoryLine finish action that Params[0] = 0
        {
            if (StoryLineData.CurStoryLineId == 0) return;
            GameData.StoryLineData.TryGetValue(StoryLineData.CurStoryLineId, out var storyExcel);
            if (storyExcel == null) return;

            if (Player.MissionManager!.GetSubMissionStatus(storyExcel.EndCondition.Param) == Enums.MissionPhaseEnum.Finish)
            {
                FinishStoryLine();
            }
        }

        public void FinishStoryLine(int entryId = 0, int anchorGroupId = 0, int anchorId = 0, bool tp = true)
        {
            if (StoryLineData.CurStoryLineId == 0) return;

            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);

            // delete old & reset
            StoryLineData.RunningStoryLines.Remove(StoryLineData.CurStoryLineId);
            StoryLineData.CurStoryLineId = 0;

            StoryLineData.OldPlaneId = 0;
            StoryLineData.OldEntryId = 0;
            StoryLineData.OldFloorId = 0;
            StoryLineData.OldPos = new();
            StoryLineData.OldRot = new();

            Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));
            Player.SendPacket(new PacketStoryLineInfoScNotify(Player));
            Player.SendPacket(new PacketChangeStoryLineFinishScNotify(0, ChangeStoryLineAction.None));

            if (tp)
            {
                if (entryId > 0)
                {
                    Player.EnterMissionScene(entryId, anchorGroupId, anchorId, true, ChangeStoryLineAction.None);
                }
                else
                {
                    Player.LoadScene(StoryLineData.OldPlaneId, StoryLineData.OldFloorId, StoryLineData.OldEntryId, StoryLineData.OldPos, StoryLineData.OldRot, true, ChangeStoryLineAction.FinishAction);
                }
            }
        }

        public void OnLogin()
        {
            if (StoryLineData.CurStoryLineId == 0) return;

            Player.SendPacket(new PacketStoryLineInfoScNotify(Player));
        }
    }
}
