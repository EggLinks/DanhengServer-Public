using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;

namespace EggLink.DanhengServer.Game.ChessRogue
{
    public class ChessRogueManager(PlayerInstance player) : BasePlayerManager(player)
    {
        public ChessRogueNousData ChessRogueNousData { get; private set; } = DatabaseHelper.Instance!.GetInstanceOrCreateNew<ChessRogueNousData>(player.Uid);

        public ChessRogueInstance? RogueInstance { get; set; }

        #region Game Management

        public void StartRogue(int aeonId, List<uint> avatarIds, int areaId, int branchId, List<int> difficultyIds, List<int> disableAeonIdList)
        {
            GameData.RogueNousAeonData.TryGetValue(aeonId, out var aeonData);
            GameData.RogueDLCAreaData.TryGetValue(areaId, out var areaData);
            if (aeonData == null || areaData == null) return;

            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupChessRogue, avatarIds.Select(x => (int)x).ToList());
            Player.LineupManager!.GainMp(5, false);
            Player.SendPacket(new PacketSyncLineupNotify(Player.LineupManager!.GetCurLineup()!));

            foreach (var id in avatarIds)
            {
                Player.AvatarManager!.GetAvatar((int)id)?.SetCurHp(10000, true);
                Player.AvatarManager!.GetAvatar((int)id)?.SetCurSp(10000, true);
            }

            var difficultyLevel = difficultyIds.Select(x => GameData.RogueNousDifficultyLevelData[x]).ToList();

            var instance = new ChessRogueInstance(Player, areaData, aeonData, areaData.RogueVersionId, branchId)
            {
                DisableAeonIds = disableAeonIdList,
                DifficultyLevel = difficultyLevel,
            };

            RogueInstance = instance;

            instance.EnterCell(instance.StartCell);

            Player.SendPacket(new PacketChessRogueStartScRsp(Player));
        }

        #endregion

        #region Dice Management

        public ChessRogueNousDiceData GetDice(int branchId)
        {
            ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);

            if (diceData == null)  // set to default
            {
                var branch = GameData.RogueNousDiceBranchData[branchId];
                var surface = branch.GetDefaultSurfaceList();
                return SetDice(branchId, surface.Select((id, i) => new { id, i }).ToDictionary(x => x.i + 1, x => x.id));  // convert to dictionary
            }

            return diceData;
        }

        public ChessRogueNousDiceData SetDice(int branchId, Dictionary<int, int> surfaceId)
        {
            ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);

            if (diceData == null)
            {
                diceData = new ChessRogueNousDiceData()
                {
                    BranchId = branchId,
                    Surfaces = surfaceId,
                };

                ChessRogueNousData.RogueDiceData[branchId] = diceData;
            }
            else
            {
                diceData.Surfaces = surfaceId;
            }

            DatabaseHelper.Instance!.UpdateInstance(ChessRogueNousData);

            return diceData;
        }

        public ChessRogueNousDiceData SetDice(int branchId, int index, int surfaceId)
        {
            ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);
            if (diceData == null)
            {
                // set to default
                var branch = GameData.RogueNousDiceBranchData[branchId];
                var surface = branch.GetDefaultSurfaceList();
                surface[index] = surfaceId;

                return SetDice(branchId, surface.Select((id, i) => new { id, i }).ToDictionary(x => x.i + 1, x => x.id));  // convert to dictionary
            } else
            {
                diceData.Surfaces[index] = surfaceId;
                DatabaseHelper.Instance!.UpdateInstance(ChessRogueNousData);

                return diceData;
            }
        }

        public ChessRogueNousDiceData SetDice(ChessRogueDice dice)
        {
            var branchId = (int)dice.BranchId;
            ChessRogueNousData.RogueDiceData.TryGetValue(branchId, out var diceData);
            if (diceData == null)
            {
                // set to default
                var branch = GameData.RogueNousDiceBranchData[branchId];
                var surface = branch.GetDefaultSurfaceList();

                foreach (var d in dice.SurfaceList)
                {
                    surface[(int)d.Index - 1] = (int)d.SurfaceId;
                }

                return SetDice(branchId, surface.Select((id, i) => new { id, i }).ToDictionary(x => x.i + 1, x => x.id));  // convert to dictionary
            }
            else
            {
                foreach (var d in dice.SurfaceList)
                {
                    diceData.Surfaces[(int)d.Index] = (int)d.SurfaceId;
                }

                DatabaseHelper.Instance!.UpdateInstance(ChessRogueNousData);

                return diceData;
            }
        }

        #endregion

        #region Serialization

        public ChessRogueGetInfo ToGetInfo()
        {
            var info = new ChessRogueGetInfo
            {
                AeonInfo = ToAeonInfo(),
                DiceInfo = ToDiceInfo(),
                RogueTalentInfo = ToTalentInfo(),
                RogueDifficultyInfo = new(),
            };

            foreach (var area in GameData.RogueDLCAreaData.Keys)
            {
                info.AreaIdList.Add((uint)area);
                info.ExploredAreaIdList.Add((uint)area);
            }

            foreach (var item in GameData.RogueNousDifficultyLevelData.Keys)
            {
                info.RogueDifficultyInfo.DifficultyId.Add((uint)item);
            }

            return info;
        }

        public ChessRogueCurrentInfo ToCurrentInfo()
        {
            if (RogueInstance != null) return RogueInstance.ToProto();
            var info = new ChessRogueCurrentInfo
            {
                RogueVersionId = 201,
                LevelInfo = ToLevelInfo(),
                RogueAeonInfo = ToRogueAeonInfo(),
                RogueDiceInfo = ToRogueDiceInfo(),
                RogueDifficultyInfo = new(),
                GameMiracleInfo = new() { MiracleInfo = new() },  // needed for avoiding null reference exception （below 4 lines）
                RogueBuffInfo = new() { BuffInfo = new() },
                PendingAction = new(),
                RogueLineupInfo = ToLineupInfo(),
                RogueVirtualItem = new(),
            };

            info.RogueCurrentInfo.AddRange(ToGameInfo());

            return info;
        }

        public ChessRogueQueryInfo ToQueryInfo()
        {
            var info = new ChessRogueQueryInfo
            {
                AeonInfo = ToAeonInfo(),
                RogueTalentInfo = ToTalentInfo(),
                RogueDifficultyInfo = new(),
                DiceInfo = ToDiceInfo(),
            };

            foreach (var area in GameData.RogueDLCAreaData.Keys)
            {
                info.AreaIdList.Add((uint)area);
                info.ExploredAreaIdList.Add((uint)area);
            }

            foreach (var item in GameData.RogueNousDifficultyLevelData.Keys)
            {
                info.RogueDifficultyInfo.DifficultyId.Add((uint)item);
            }

            return info;
        }

        public ChessRogueLevelInfo ToLevelInfo()
        {
            var proto = new ChessRogueLevelInfo()
            {
                AreaInfo = new()
                {
                    Cell = new(),
                    DOKMJNIHNOO = new(),
                }
            };

            foreach (var area in GameData.RogueDLCAreaData.Keys)
            {
                proto.ExploredAreaIdList.Add((uint)area);
            }


            return proto;
        }

        public ChessRogueQueryAeonInfo ToAeonInfo()
        {
            var proto = new ChessRogueQueryAeonInfo();

            foreach (var aeon in GameData.RogueNousAeonData.Values)
            {
                if (aeon.AeonID > 7) continue;
                proto.AeonList.Add(new ChessRogueQueryAeon()
                {
                    AeonId = (uint)aeon.AeonID,
                });
            }

            return proto;
        }

        public ChessRogueAeonInfo ToRogueAeonInfo()
        {
            var proto = new ChessRogueAeonInfo()
            {
                AeonInfo = ToAeonInfo(),
            };


            foreach (var aeon in GameData.RogueNousAeonData.Values)
            {
                if (aeon.AeonID > 8) continue;
                proto.AeonIdList.Add((uint)aeon.AeonID);
            }

            return proto;
        }

        public ChessRogueQueryDiceInfo ToDiceInfo()
        {
            var proto = new ChessRogueQueryDiceInfo()
            {
                DicePhase = ChessRogueNousDicePhase.PhaseTwo,
            };

            foreach (var branch in GameData.RogueNousDiceSurfaceData.Keys)
            {
                proto.SurfaceIdList.Add((uint)branch);
            }

            foreach (var dice in GameData.RogueNousDiceBranchData)
            {
                proto.DiceList.Add(GetDice(dice.Key).ToProto());
            }

            for (var i = 1; i < 7; i++)
            {
                proto.LAHGNGJAOFH.Add((uint)i, i % 3 == 0);
            }
            proto.LAHGNGJAOFH[5] = true;

            return proto;
        }

        public ChessRogueDiceInfo ToRogueDiceInfo()
        {
            var proto = new ChessRogueDiceInfo()
            {
                IsValid = true,
            };

            return proto;
        }

        public List<ChessRogueGameInfo> ToGameInfo()
        {
            var proto = new List<ChessRogueGameInfo>
            {
                new()
                {
                    RogueAeonInfo = new()
                },
                new()
                {
                    GameItemInfo = new()
                },
                new()
                {
                    GameMiracleInfo = new()
                    {
                        MiracleInfo = new()
                    }
                },
                new()
                {
                    RogueBuffInfo = new()
                    {
                        BuffInfo = new()
                    }
                }
            };

            return proto;
        }

        public ChessRogueTalentInfo ToTalentInfo()
        {
            var talentInfo = new RogueTalentInfo();

            foreach (var talent in GameData.RogueNousTalentData.Values)
            {
                talentInfo.RogueTalentList.Add(new RogueTalent()
                {
                    TalentId = (uint)talent.TalentID,
                    Status = RogueTalentStatus.Enable
                });
            }

            var proto = new ChessRogueTalentInfo()
            {
                TalentInfo = talentInfo,
            };

            return proto;
        }

        public ChessRogueLineupInfo ToLineupInfo()
        {
            var proto = new ChessRogueLineupInfo()
            {
                ReviveInfo = new()
                {
                    RogueReviveCost = new()
                }
            };

            return proto;
        }

        #endregion
    }
}
