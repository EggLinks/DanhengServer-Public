using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Game.Rogue.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;

namespace EggLink.DanhengServer.Game.Rogue
{
    public class RogueInstance : BaseRogueInstance
    {
        #region Properties

        public RogueStatus Status { get; set; } = RogueStatus.Doing;
        public int CurReachedRoom { get; set; } = 0;

        public RogueAeonExcel AeonExcel { get; set; }
        public RogueAreaConfigExcel AreaExcel { get; set; }
        public Dictionary<int, RogueRoomInstance> RogueRooms { get; set; } = [];
        public RogueRoomInstance? CurRoom { get; set; }
        public int StartSiteId { get; set; } = 0;

        #endregion

        #region Initialization

        public RogueInstance(RogueAreaConfigExcel areaExcel, RogueAeonExcel aeonExcel, PlayerInstance player) : base(player, 101, aeonExcel.RogueBuffType)
        {
            AreaExcel = areaExcel;
            AeonExcel = aeonExcel;
            AeonId = aeonExcel.AeonID;
            Player = player;
            CurLineup = player.LineupManager!.GetCurLineup()!;
            EventManager = new(player, this);

            foreach (var item in areaExcel.RogueMaps.Values)
            {
                RogueRooms.Add(item.SiteID, new(item));
                if (item.IsStart)
                {
                    StartSiteId = item.SiteID;
                }
            }

            // add bonus
            var action = new RogueActionInstance()
            {
                QueuePosition = CurActionQueuePosition,
            };
            action.SetBonus();

            RogueActions.Add(CurActionQueuePosition, action);
        }

        #endregion

        #region Buffs

        public override void RollBuff(int amount)
        {
            if (CurRoom!.Excel.RogueRoomType == 6)
            {
                RollBuff(amount, 100003, 2);  // boss room
                RollMiracle(1);
            }
            else
            {
                RollBuff(amount, 100005);
            }
        }

        public void AddAeonBuff()
        {
            if (AeonBuffPending) return;
            if (CurAeonBuffCount + CurAeonEnhanceCount >= 4)
            {
                // max aeon buff count
                return;
            }
            int curAeonBuffCount = 0;  // current path buff count
            int hintId = AeonId * 100 + 1;
            var enhanceData = GameData.RogueAeonEnhanceData[AeonId];
            var buffData = GameData.RogueAeonBuffData[AeonId];
            foreach (var buff in RogueBuffs)
            {
                if (buff.BuffExcel.RogueBuffType == AeonExcel.RogueBuffType)
                {
                    if (!buff.BuffExcel.IsAeonBuff)
                    {
                        curAeonBuffCount++;
                    }
                    else
                    {
                        hintId++;  // next hint
                        enhanceData.Remove(buff.BuffExcel);
                    }
                }
            }

            var needAeonBuffCount = (CurAeonBuffCount + CurAeonEnhanceCount) switch
            {
                0 => 3,
                1 => 6,
                2 => 10,
                3 => 14,
                _ => 100,
            };

            if (curAeonBuffCount >= needAeonBuffCount)
            {
                RogueBuffSelectMenu menu = new(this)
                {
                    QueueAppend = 2,
                    HintId = hintId,
                    RollMaxCount = 0,
                    RollFreeCount = 0,
                    IsAeonBuff = true,
                };
                if (CurAeonBuffCount < 1)
                {
                    CurAeonBuffCount++;
                    // add aeon buff
                    menu.RollBuff([buffData], 1);
                }
                else
                {
                    CurAeonEnhanceCount++;
                    // add enhance buff
                    menu.RollBuff(enhanceData, enhanceData.Count);
                }

                var action = menu.GetActionInstance();
                RogueActions.Add(action.QueuePosition, action);

                AeonBuffPending = true;
                UpdateMenu();
            }
        }

        #endregion

        #region Methods

        public override void UpdateMenu()
        {
            base.UpdateMenu();


            AddAeonBuff();  // check if aeon buff can be added
        }

        public RogueRoomInstance? EnterRoom(int siteId)
        {
            var prevRoom = CurRoom;
            if (prevRoom != null)
            {
                if (!prevRoom.NextSiteIds.Contains(siteId))
                {
                    return null;
                }
                prevRoom.Status = RogueRoomStatus.Finish;
                // send
                Player.SendPacket(new PacketSyncRogueMapRoomScNotify(prevRoom, AreaExcel.MapId));
            }

            // next room
            CurReachedRoom++;
            CurRoom = RogueRooms[siteId];
            CurRoom.Status = RogueRoomStatus.Play;

            Player.EnterScene(CurRoom.Excel.MapEntrance, 0, false);

            // move
            AnchorInfo? anchor = Player.SceneInstance!.FloorInfo?.GetAnchorInfo(CurRoom.Excel.GroupID, 1);
            if (anchor != null)
            {
                Player.Data.Pos = anchor.ToPositionProto();
                Player.Data.Rot = anchor.ToRotationProto();
            }

            // send
            Player.SendPacket(new PacketSyncRogueMapRoomScNotify(CurRoom, AreaExcel.MapId));

            // call event
            EventManager?.OnNextRoom();
            foreach (var miracle in RogueMiracles.Values)
            {
                miracle.OnEnterNextRoom();
            }

            return CurRoom;
        }

        public void LeaveRogue()
        {
            Player.RogueManager!.RogueInstance = null;
            Player.EnterScene(801120102, 0, false);
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);
        }

        public void QuitRogue()
        {
            Status = RogueStatus.Finish;
            Player.SendPacket(new PacketSyncRogueStatusScNotify(Status));
            Player.SendPacket(new PacketSyncRogueFinishScNotify(ToFinishInfo()));
        }

        #endregion

        #region Handlers

        public override void OnBattleStart(BattleInstance battle)
        {
            base.OnBattleStart(battle);

            GameData.RogueMapData.TryGetValue(AreaExcel.MapId, out var mapData);
            if (mapData != null)
            {
                mapData.TryGetValue(CurRoom!.SiteId, out var mapInfo);
                if (mapInfo != null && mapInfo.LevelList.Count > 0)
                {
                    battle.CustomLevel = mapInfo.LevelList.RandomElement();
                }
            }
        }

        public override void OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
        {
            foreach (var miracle in RogueMiracles.Values)
            {
                miracle.OnEndBattle(battle);
            }

            if (req.EndStatus != BattleEndStatus.BattleEndWin)
            {
                // quit
                QuitRogue();
                return;
            }

            if (CurRoom!.NextSiteIds.Count == 0)
            {
                // last room
                IsWin = true;
                Player.SendPacket(new PacketSyncRogueExploreWinScNotify());
            }
            else
            {
                RollBuff(battle.Stages.Count);
                GainMoney(Random.Shared.Next(20, 60) * battle.Stages.Count);
            }
        }

        #endregion

        #region Serialization

        public RogueCurrentInfo ToProto()
        {
            var proto = new RogueCurrentInfo()
            {
                Status = Status,
                GameMiracleInfo = ToMiracleInfo(),
                RogueAeonInfo = ToAeonInfo(),
                RogueLineupInfo = ToLineupInfo(),
                RogueBuffInfo = ToBuffInfo(),
                RogueVirtualItem = ToVirtualItemInfo(),
                MapInfo = ToMapInfo(),
                ModuleInfo = new()
                {
                    ModuleIdList = { 1, 2, 3, 4, 5 },
                },
                IsWin = IsWin,
            };

            if (RogueActions.Count > 0)
            {
                proto.PendingAction = RogueActions.First().Value.ToProto();
            } else
            {
                proto.PendingAction = new();
            }

            return proto;
        }

        public GameAeonInfo ToAeonInfo()
        {
            return new()
            {
                AeonId = (uint)AeonId,
                IsUnlocked = AeonId != 0,
                UnlockedAeonEnhanceNum = (uint)(AeonId != 0 ? 3 : 0)
            };
        }

        public RogueLineupInfo ToLineupInfo()
        {
            var proto = new RogueLineupInfo();

            foreach (var avatar in CurLineup!.BaseAvatars!)
            {
                proto.BaseAvatarIdList.Add((uint)avatar.BaseAvatarId);
            }

            proto.ReviveInfo = new()
            {
                RogueReviveCost = new()
                {
                    ItemList = {
                        new ItemCost() {
                            PileItem = new PileItem() {
                                ItemId = 31,
                                ItemNum = (uint)CurReviveCost
                            }
                        },
                    }
                }
            };

            return proto;
        }

        public RogueVirtualItem ToVirtualItemInfo()
        {
            return new()
            {
                RogueMoney = (uint)CurMoney,
            };
        }

        public RogueMapInfo ToMapInfo()
        {
            var proto = new RogueMapInfo()
            {
                CurSiteId = (uint)CurRoom!.SiteId,
                CurRoomId = (uint)CurRoom!.RoomId,
                AreaId = (uint)AreaExcel.RogueAreaID,
                MapId = (uint)AreaExcel.MapId,
            };

            foreach (var room in RogueRooms)
            {
                proto.RoomList.Add(room.Value.ToProto());
            }

            return proto;
        }

        public GameMiracleInfo ToMiracleInfo()
        {
            var proto = new GameMiracleInfo()
            {
                GameMiracleInfo_ = new()
                {
                    MiracleList = { },  // for the client serialization
                }
            };

            foreach (var miracle in RogueMiracles.Values)
            {
                proto.GameMiracleInfo_.MiracleList.Add(miracle.ToProto());
            }

            return proto;
        }

        public RogueBuffInfo ToBuffInfo()
        {
            var proto = new RogueBuffInfo()
            {
                MazeBuffList = { }
            };

            foreach (var buff in RogueBuffs)
            {
                proto.MazeBuffList.Add(buff.ToProto());
            }

            return proto;
        }

        public RogueFinishInfo ToFinishInfo()
        {
            AreaExcel.ScoreMap.TryGetValue(CurReachedRoom, out var score);
            var prev = Player.RogueManager!.ToRewardProto();
            Player.RogueManager!.AddRogueScore(score);
            var next = Player.RogueManager!.ToRewardProto();

            return new()
            {
                ScoreId = (uint)score,
                TotalScore = (uint)score,
                PrevRewardInfo = prev,
                NextRewardInfo = next,
                AreaId = (uint)AreaExcel.RogueAreaID,
                FinishedRoomCount = (uint)CurReachedRoom,
                ReachedRoomCount = (uint)CurReachedRoom,
                IsWin = IsWin,
                Record = new()
                {
                    AvatarList = { CurLineup!.BaseAvatars!.Select(avatar => new RogueRecordAvatar()
                    {
                        Id = (uint)avatar.BaseAvatarId,
                        AvatarType = AvatarType.AvatarFormalType,
                        Level = (uint)(Player.AvatarManager!.GetAvatar(avatar.BaseAvatarId)?.Level ?? 0),
                        Slot = (uint)CurLineup!.BaseAvatars!.IndexOf(avatar),
                    }) },
                    BuffList = { RogueBuffs.Select(buff => buff.ToProto()) },
                    MiracleList = { RogueMiracles.Values.Select(miracle => (uint)miracle.MiracleId) },
                }
            };
        }

        #endregion
    }
}
