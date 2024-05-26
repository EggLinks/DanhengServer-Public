using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Rogue;
using EggLink.DanhengServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.ChessRogue
{
    public class ChessRogueInstance : BaseRogueInstance
    {
        public RogueDLCAreaExcel AreaExcel { get; set; }
        public RogueNousAeonExcel AeonExcel { get; set; }
        public List<RogueNousDifficultyLevelExcel> DifficultyLevel { get; set; } = [];
        public List<RogueDLCBossDecayExcel> BossBuff { get; set; } = [];
        public int BossAeonId { get; set; }
        public List<RogueDLCDifficultyExcel> DifficultyExcel { get; set; } = [];
        public ChessRogueDiceInstance DiceInstance { get; set; }

        public Dictionary<int, ChessRogueCellInstance> RogueCells { get; set; } = [];
        public ChessRogueCellInstance? CurCell { get; set; }
        public List<ChessRogueCellInstance> HistoryCell { get; set; } = [];
        public int StartCell { get; set; } = 0;

        public List<int> Layers { get; set; } = [];
        public Dictionary<int, List<int>>? CurLayerData { get; set; }
        public int CurLayer { get; set; } = 0;
        public ChessRogueLevelStatusType CurLevelStatus { get; set; } = ChessRogueLevelStatusType.ChessRogueLevelProcessing;

        public int ActionPoint { get; set; } = 15;

        public List<int> DisableAeonIds { get; set; } = [];

        public ChessRogueInstance(PlayerInstance player, RogueDLCAreaExcel areaExcel, RogueNousAeonExcel aeonExcel, int rogueVersionId, int branchId) : base(player, rogueVersionId, aeonExcel.RogueBuffType)
        {
            CurLineup = player.LineupManager!.GetCurLineup()!;
            Player = player;
            AeonId = aeonExcel.AeonID;
            AreaExcel = areaExcel;
            AeonExcel = aeonExcel;
            DiceInstance = new(this, player.ChessRogueManager!.GetDice(branchId));
            Layers = areaExcel.LayerIDList;
            CurLayer = Layers.First();
            EventManager = new(player, this);

            foreach (var difficulty in areaExcel.DifficultyID)
            {
                if (GameData.RogueDLCDifficultyData.TryGetValue(difficulty, out var diff))
                    DifficultyExcel.Add(diff);
            }

            var action = new RogueActionInstance()
            {
                QueuePosition = CurActionQueuePosition,
            };
            action.SetBonus();

            RogueActions.Add(CurActionQueuePosition, action);

            // generate cells
            GenerateLayer();
        }

        #region Buff Management

        public override void RollBuff(int amount)
        {
            if (CurCell!.CellType == 11)
            {
                RollBuff(amount, 100003, 2);  // boss room
                RollMiracle(1);
            }
            else
            {
                RollBuff(amount, 100005);
            }
        }

        public void AddAeonBuff()  // TODO: support for aeon cross
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
        public override void UpdateMenu()
        {
            base.UpdateMenu();


            AddAeonBuff();  // check if aeon buff can be added
        }

        #endregion

        #region Map Management

        public void EnterNextLayer()
        {
            HistoryCell.Clear();
            CurLayer = Layers[Layers.IndexOf(CurLayer) + 1];
            CurLevelStatus = ChessRogueLevelStatusType.ChessRogueLevelProcessing;

            GenerateLayer();
            EnterCell(StartCell);

            Player.SendPacket(new PacketChessRogueEnterNextLayerScRsp(this));
        }

        public void GenerateLayer()
        {
            GameData.ChessRogueLayerGenData.TryGetValue(CurLayer, out var layerGenData);
            if (layerGenData == null)
            {
                return;
            }

            CurLayerData = layerGenData;

            RogueCells.Clear();
            CurCell = null;

            foreach (var column in layerGenData)
            {
                if (column.Key == -1)
                {
                    continue;
                }
                foreach (var row in column.Value)
                {
                    var cell = new ChessRogueCellInstance(this)
                    {
                        Column = column.Key,
                        Row = row,
                    };
                    RogueCells.Add(column.Key * 100 + row, cell);

                    if (column.Key == 2 && column.Value.IndexOf(row) == 0)
                    {
                        if (Layers.IndexOf(CurLayer) == 0)
                        {
                            cell.CellType = 3;
                        } else
                        {
                            cell.CellType = 2;
                        }
                        StartCell = column.Key * 100 + row;
                    }

                    if (column.Key == 2 && column.Value.IndexOf(row) == column.Value.Count - 1)  // last cell
                    {
                        if (Layers.IndexOf(CurLayer) == Layers.Count - 1)
                        {
                            cell.CellType = 15;
                        } else
                        {
                            cell.CellType = 11;
                        }
                        cell.Init();
                    }
                }
            }
        }

        public void EnterCell(int id, int monsterId = 0)
        {
            if (RogueCells.TryGetValue(id, out var cell))
            {
                if (monsterId > 0)
                {
                    cell.SelectMonsterId = monsterId;
                    if (BossAeonId == 0)
                    {
                        BossAeonId = cell.GetBossAeonId(monsterId);
                    }

                    var excel = cell.GetBossDecayExcel(monsterId);
                    if (excel != null)
                    {
                        BossBuff.Add(excel);
                    }
                }

                CurCell = cell;
                cell.CellStatus = ChessRogueBoardCellStatus.Finish;

                Player.EnterScene(cell.GetEntryId(), 0, false);
                Player.MoveTo(new EntityMotion()
                {
                    Motion = new()
                    {
                        Rot = cell.CellConfig!.ToRotation().ToProto(),
                        Pos = cell.CellConfig!.ToPosition().ToProto(),
                    }
                });

                HistoryCell.Add(cell);

                DiceInstance.CurSurfaceId = 0;
                DiceInstance.DiceStatus = ChessRogueDiceStatus.ChessRogueDiceIdle;
                Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
            }
        }

        public void SelectCell(int cellId)
        {
            var cell = RogueCells[cellId];
            if (cell.CellStatus == ChessRogueBoardCellStatus.Finish)
            {
                return;
            }

            cell.CellStatus = ChessRogueBoardCellStatus.Selected;

            Player.SendPacket(new PacketChessRogueCellUpdateNotify(cell, CurLayerData![-1][0]));
            CostActionPoint(1);

            Player.SendPacket(new PacketChessRogueSelectCellScRsp(cellId));
        }

        #endregion

        #region Dice Management

        public void RollDice()
        {
            DiceInstance.RollDice();

            Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
            Player.SendPacket(new PacketChessRogueRollDiceScRsp(DiceInstance));
        }

        public void ReRollDice()
        {
            DiceInstance.RerollTimes--;
            DiceInstance.RollDice();

            Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
            Player.SendPacket(new PacketChessRogueReRollDiceScRsp(DiceInstance));
        }

        public void CheatDice(int surfaceId)
        {
            DiceInstance.CheatTimes--;
            DiceInstance.CurSurfaceId = surfaceId;

            Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
            Player.SendPacket(new PacketChessRogueCheatRollScRsp(DiceInstance, surfaceId));
        }

        public void ConfirmRoll()
        {
            DiceInstance.DiceStatus = ChessRogueDiceStatus.ChessRogueDiceConfirmed;

            Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
            Player.SendPacket(new PacketChessRogueConfirmRollScRsp(DiceInstance));

            //var cellIds = new List<int>();

            //foreach (var cell in RogueCells)
            //{
            //    if (cell.Key == CurCell!.GetCellId() || cell.Value.CellStatus == ChessRogueBoardCellStatus.Finish)
            //    {
            //        continue;
            //    }
            //    cellIds.Add(cell.Key);
            //}

            //Player.SendPacket(new PacketChessRogueUpdateAllowedSelectCellScNotify(CurLayerData![-1][0], cellIds));
        }

        #endregion

        #region Action Management

        public void CostActionPoint(int cost)
        {
            ActionPoint -= cost;

            Player.SendPacket(new PacketChessRogueUpdateActionPointScNotify(ActionPoint));
        }

        public void QuitRogue()
        {
            CurLevelStatus = ChessRogueLevelStatusType.ChessRogueLevelFinish;
            Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));

            Player.ChessRogueManager!.RogueInstance = null;
            Player.EnterScene(801120102, 0, false);
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);

            Player.SendPacket(new PacketChessRogueQuitScRsp(this));
        }

        public void LeaveRogue()
        {
            CurLevelStatus = ChessRogueLevelStatusType.ChessRogueLevelFinish;
            Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));

            Player.ChessRogueManager!.RogueInstance = null;
            Player.EnterScene(801120102, 0, false);
            Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);

            Player.SendPacket(new PacketChessRogueLeaveScRsp(this));
        }

        #endregion

        public override void OnBattleStart(BattleInstance battle)
        {
            base.OnBattleStart(battle);

            if (DifficultyExcel.Count > 0)
            {
                battle.CustomLevel = DifficultyExcel.RandomElement().LevelList.RandomElement();
            }

            if (ActionPoint < 0)
            {
                GameData.ActionPointOverdrawData.TryGetValue(Math.Max(ActionPoint, -50), out var overdrawData);
                if (overdrawData != null)
                {
                    battle.Buffs.Add(new MazeBuff(overdrawData.MazeBuff, 1, -1)
                    {
                        WaveFlag = -1,
                    });
                }
            }

            CalculateDifficulty(battle);

            if (CurCell!.CellType == 15)
            {
                var buffList = new List<int>();
                foreach (var buff in BossBuff)
                {
                    if (buff.EffectType == Enums.Rogue.BossDecayEffectTypeEnum.AddMazeBuffList)
                    {
                        buffList.SafeAddRange(buff.EffectParamList);  // add buff
                    } else
                    {
                        foreach (var param in buff.EffectParamList)  // remove buff
                        {
                            buffList.Remove(param);
                        }
                    }
                }

                foreach (var buffId in buffList)
                {
                    battle.Buffs.Add(new MazeBuff(buffId, 1, -1)
                    {
                        WaveFlag = -1,
                    });
                }
            }
        }

        public void CalculateDifficulty(BattleInstance battle)
        {
            if (DifficultyExcel.Count > 0)
            {
                List<int> difficultyIds = [];
                List<MazeBuff> buffs = [];

                foreach (var difficulty in DifficultyLevel)
                {
                    difficultyIds.Add(difficulty.DifficultyID);
                }

                if (difficultyIds.Contains(103))
                {
                    buffs.Add(new MazeBuff(610121, 1, -1)
                    {
                        WaveFlag = -1
                    });
                }

                if (difficultyIds.Contains(105))
                {
                    buffs.Add(new MazeBuff(610121, 1, -1)
                    {
                        WaveFlag = -1
                    });
                }

                var attributeBuffId = 0;

                if (difficultyIds.Contains(106))
                {
                    attributeBuffId = 650204;
                } else if (difficultyIds.Contains(104))
                {
                    attributeBuffId = 650203;
                } else if (difficultyIds.Contains(102))
                {
                    attributeBuffId = 650202;
                } else if (difficultyIds.Contains(101))
                {
                    attributeBuffId = 650201;
                }

                buffs.Add(new MazeBuff(attributeBuffId, 1, -1)
                {
                    WaveFlag = -1,
                    DynamicValues = { { "_RogueLayer", Layers.IndexOf(CurLayer) + 1 } }
                });

                foreach (var buff in buffs)
                {
                    battle.Buffs.Add(buff);
                }
            }
        }

        public override void OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
        {
            base.OnBattleEnd(battle, req);

            if (req.EndStatus != BattleEndStatus.BattleEndWin)
            {
                // quit
                CurLevelStatus = ChessRogueLevelStatusType.ChessRogueLevelFailed;
                Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));
                return;
            }

            RollBuff(battle.Stages.Count);

            if (CurCell!.CellType == 11)
            {
                Player.SendPacket(new PacketChessRogueLayerAccountInfoNotify(this));
            }
            else if (CurCell!.CellType == 15)
            {
                CurLevelStatus = ChessRogueLevelStatusType.ChessRogueLevelFinish;
                Player.SendPacket(new PacketChessRogueLayerAccountInfoNotify(this));
                Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));
            }
        }

        #region Serialization

        public ChessRogueCurrentInfo ToProto()
        {
            var proto = new ChessRogueCurrentInfo()
            {
                GameMiracleInfo = ToMiracleInfo(),
                RogueBuffInfo = ToBuffInfo(),
                RogueAeonInfo = ToAeonInfo(),
                RogueVersionId = (uint)RogueVersionId,
                RogueDiceInfo = DiceInstance.ToProto(),
                RogueLineupInfo = ToLineupInfo(),
                RogueDifficultyInfo = ToDifficultyInfo(),
                RogueVirtualItem = ToVirtualItemInfo(),
                LevelInfo = ToLevelInfo(),
            };

            if (RogueActions.Count > 0)
            {
                proto.PendingAction = RogueActions.First().Value.ToProto();
            } else
            {
                proto.PendingAction = new();
            }

            proto.RogueCurrentInfo.AddRange(ToGameInfo());

            return proto;
        }

        public ChessRoguePlayerInfo ToPlayerProto()
        {
            var playerInfo = new ChessRoguePlayerInfo()
            {
                Lineup = Player.LineupManager!.GetCurLineup()!.ToProto(),
                Scene = Player.SceneInstance!.ToProto(),
            };

            return playerInfo;
        }

        public ChessRogueQueryGameInfo ToRogueGameInfo()
        {
            var proto = new ChessRogueQueryGameInfo()
            {
                RogueVersionId = (uint)RogueVersionId,
            };

            proto.RogueCurrentInfo.AddRange(ToGameInfo());

            return proto;
        }

        public ChessRogueMiracleInfo ToMiracleInfo()
        {
            var proto = new ChessRogueMiracleInfo()
            {
                MiracleInfo = new()
            };

            proto.MiracleInfo.MiracleList.AddRange(RogueMiracles.Select(x => x.Value.ToGameMiracleProto()).ToList());

            return proto;
        }

        public ChessRogueBuffInfo ToBuffInfo()
        {
            var proto = new ChessRogueBuffInfo()
            {
                BuffInfo = new()
            };

            proto.BuffInfo.BuffList.AddRange(RogueBuffs.Select(x => x.ToCommonProto()).ToList());

            return proto;
        }

        public ChessRogueAeonInfo ToAeonInfo()
        {
            var proto = new ChessRogueAeonInfo()
            {
                AeonId = (uint)AeonId,
            };

            return proto;
        }

        public ChessRogueGameAeonInfo ToGameAeonInfo()
        {
            var proto = new ChessRogueGameAeonInfo()
            {
                AeonId = (uint)AeonId,
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
                    {
                        ItemList = { new ItemCost()
                        {
                            PileItem = new()
                            {
                                ItemId = 31,
                                ItemNum = (uint)CurReviveCost
                            }
                        } }
                    }
                },
            };

            foreach (var avatar in CurLineup!.BaseAvatars!)
            {
                proto.AvatarList.Add(new ChessRogueLineupAvatarInfo()
                {
                    AvatarId = (uint)avatar.BaseAvatarId,
                });
            }

            return proto;
        }

        public ChessRogueGameItemInfo ToGameItemInfo()
        {
            var proto = new ChessRogueGameItemInfo();

            proto.ItemMap.Add(31, (uint)CurMoney);

            return proto;
        }

        public List<ChessRogueGameInfo> ToGameInfo()
        {
            var proto = new List<ChessRogueGameInfo>
            {
                new()
                {
                    RogueDifficultyInfo = ToDifficultyLevelInfo()
                },
                new()
                {
                    RogueAeonInfo = ToGameAeonInfo()
                },
                new()
                {
                    GameItemInfo = ToGameItemInfo()
                },
                new()
                {
                    GameMiracleInfo = ToMiracleInfo()
                },
                new()
                {
                    RogueBuffInfo = ToBuffInfo()
                }
            };

            return proto;
        }

        public ChessRogueCurrentDifficultyInfo ToDifficultyInfo()
        {
            var proto = new ChessRogueCurrentDifficultyInfo();

            foreach (var level in DifficultyLevel)
            {
                proto.DifficultyId.Add((uint)level.DifficultyID);
            }

            return proto;
        }

        public ChessRogueDifficultyLevelInfo ToDifficultyLevelInfo()
        {
            var proto = new ChessRogueDifficultyLevelInfo();

            foreach (var level in DifficultyLevel)
            {
                proto.DifficultyId.Add((uint)level.DifficultyID);
            }

            return proto;
        }

        public RogueVirtualItem ToVirtualItemInfo()
        {
            return new()
            {
                RogueMoney = (uint)CurMoney
            };
        }

        public ChessRogueLevelInfo ToLevelInfo()
        {
            List<uint> canSelected = [];
            foreach (var cell in RogueCells)
            {
                if (cell.Value.CellStatus == ChessRogueBoardCellStatus.Idle)
                {
                    if (cell.Value.Column == CurCell!.Column - 1 || cell.Value.Column == CurCell!.Column || cell.Value.Column == CurCell!.Column + 1)
                    {
                        if (cell.Value.Row == CurCell!.Row || cell.Value.Row == CurCell!.Row + 1)
                        {
                            canSelected.Add((uint)cell.Value.GetCellId());
                        }
                    }
                }
            }

            var proto = new ChessRogueLevelInfo()
            {
                LevelStatus = CurLevelStatus,
                ActionPoint = ActionPoint,
                Id = (uint)AreaExcel.AreaID,
                LayerId = (uint)CurLayer,
                AreaInfo = new()
                {
                    LayerStatus = ChessRogueBoardCellStatus.Processing,
                    CurId = (uint)CurCell!.GetCellId(),
                    BoardId = (uint)CurLayerData![-1][0],
                    Cell = new()
                    {
                        CellList = { RogueCells.Select(x => x.Value.ToProto()).ToList() }
                    },
                    AllowedSelectCellIdList = { canSelected },
                    HistoryCell = { HistoryCell.Select(x => x.ToHistoryProto()).ToList() },
                }
            };

            return proto;
        }

        public ChessRogueFinishInfo ToFinishInfo()
        {
            var info = new ChessRogueFinishInfo()
            {
                AreaId = (uint)AreaExcel.AreaID,
                CurLayerId = (uint)CurLayer,
                CurLineup = CurLineup!.ToProto(),
                DifficultyLevel = uint.Parse(AreaExcel.AreaID.ToString().Substring(AreaExcel.AreaID.ToString().Length - 1, 1)),
                RogueVersionId = (uint)RogueVersionId,
                RogueBuffInfo = new()
                {
                    BuffList = { RogueBuffs.Select(x => x.ToCommonProto()) }
                },
                GameMiracleInfo = new()
                {
                    MiracleList = { RogueMiracles.Select(x => x.Value.ToGameMiracleProto()) }
                },
            };

            return info;
        }

        #endregion
    }
}
