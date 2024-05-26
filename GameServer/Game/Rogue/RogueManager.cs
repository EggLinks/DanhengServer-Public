using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using EggLink.DanhengServer.Util;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue
{
    public class RogueManager(PlayerInstance player) : BasePlayerManager(player)
    {
        #region Properties

        public RogueInstance? RogueInstance { get; set; }

        #endregion

        #region Information

        /// <summary>
        /// Get the begin time and end time
        /// </summary>
        /// <returns></returns>
        public static (long, long) GetCurrentRogueTime()
        {
            // get the first day of the week
            var beginTime = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).AddHours(4);
            var endTime = beginTime.AddDays(7);
            return (beginTime.ToUnixSec(), endTime.ToUnixSec());
        }

        public int GetRogueScore() => 0;  // TODO: Implement

        public void AddRogueScore(int score)
        {

        }

        public static RogueManagerExcel? GetCurrentManager()
        {
            foreach (var manager in GameData.RogueManagerData.Values)
            {
                if (DateTime.Now >= manager.BeginTimeDate && DateTime.Now <= manager.EndTimeDate)
                {
                    return manager;
                }
            }
            return null;
        }

        #endregion

        #region Actions

        public void StartRogue(int areaId, int aeonId, List<int> disableAeonId, List<int> baseAvatarIds)
        {
            if (GetRogueInstance() != null)
            {
                return;
            }
            GameData.RogueAreaConfigData.TryGetValue(areaId, out var area);
            GameData.RogueAeonData.TryGetValue(aeonId, out var aeon);

            if (area == null || aeon == null)
            {
                return;
            }

            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupRogue, baseAvatarIds);
            Player.LineupManager!.GainMp(5, false);
            Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));

            foreach (var id in baseAvatarIds)
            {
                Player.AvatarManager!.GetAvatar(id)?.SetCurHp(10000, true);
                Player.AvatarManager!.GetAvatar(id)?.SetCurSp(5000, true);
            }

            RogueInstance = new RogueInstance(area, aeon, Player);
            RogueInstance.EnterRoom(RogueInstance.StartSiteId);

            Player.SendPacket(new PacketSyncRogueStatusScNotify(RogueInstance.Status));
            Player.SendPacket(new PacketStartRogueScRsp(Player));
        }

        public BaseRogueInstance? GetRogueInstance()
        {
            if (RogueInstance != null)
            {
                return RogueInstance;
            } else
            {
                return Player.ChessRogueManager!.RogueInstance;
            }
        }

        #endregion

        #region Serialization

        public RogueInfo ToProto()
        {
            var proto = new RogueInfo()
            {
                RogueGetInfo = ToGetProto()
            };

            if (RogueInstance != null)
            {
                proto.RogueCurrentInfo = RogueInstance.ToProto();
            }

            return proto;
        }

        public RogueGetInfo ToGetProto()
        {
            return new()
            {
                RogueScoreRewardInfo = ToRewardProto(),
                RogueAeonInfo = ToAeonInfo(),
                RogueSeasonInfo = ToSeasonProto(),
                RogueAreaInfo = ToAreaProto(),
                RogueVirtualItemInfo = ToVirtualItemProto()
            };
        }

        public RogueScoreRewardInfo ToRewardProto()
        {
            var time = GetCurrentRogueTime();

            return new()
            {
                ExploreScore = (uint)GetRogueScore(),
                PoolRefreshed = true,
                PoolId = (uint)(20 + Player.Data.WorldLevel),
                BeginTime = time.Item1,
                EndTime = time.Item2,
                HasTakenInitialScore = true
            };
        }

        public static RogueAeonInfo ToAeonInfo()
        {
            var proto = new RogueAeonInfo()
            {
                IsUnlocked = true,
                UnlockedAeonNum = (uint)GameData.RogueAeonData.Count,
                UnlockedAeonEnhanceNum = 3
            };

            proto.AeonIdList.AddRange(GameData.RogueAeonData.Keys.Select(x => (uint)x));

            return proto;
        }

        public static RogueSeasonInfo ToSeasonProto()
        {
            var manager = GetCurrentManager();
            if (manager == null)
            {
                return new RogueSeasonInfo();
            }

            return new()
            {
                Season = (uint)manager.RogueSeason,
                BeginTime = manager.BeginTimeDate.ToUnixSec(),
                EndTime = manager.EndTimeDate.ToUnixSec(),
            };
        }

        public static RogueAreaInfo ToAreaProto()
        {
            var manager = GetCurrentManager();
            if (manager == null)
            {
                return new RogueAreaInfo();
            }
            return new()
            {
                RogueAreaList = {manager.RogueAreaIDList.Select(x => new RogueArea()
                {
                    AreaId = (uint)x,
                    AreaStatus = RogueAreaStatus.FirstPass,
                    HasTakenReward = true
                })}
            };
        }

        public static RogueGetVirtualItemInfo ToVirtualItemProto()
        {
            return new()
            {
                // TODO: Implement
            };
        }

        public static RogueTalentInfo ToTalentProto()
        {
            var proto = new RogueTalentInfo();

            foreach (var talent in GameData.RogueTalentData)
            {
                proto.RogueTalentList.Add(new RogueTalent()
                {
                    TalentId = (uint)talent.Key,
                    Status = RogueTalentStatus.Enable,
                });
            }

            return proto;
        }

        #endregion
    }
}
