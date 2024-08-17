using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;
using EggLink.DanhengServer.GameServer.Game.ChessRogue.Dice;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.GameServer.Game.Rogue.Buff;
using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue;

public class ChessRogueInstance : BaseRogueInstance
{
    public ChessRogueInstance(PlayerInstance player, RogueDLCAreaExcel areaExcel, RogueNousAeonExcel aeonExcel,
        RogueSubModeEnum rogueSubMode, int branchId) : base(player, rogueSubMode, aeonExcel.RogueBuffType)
    {
        CurLineup = player.LineupManager!.GetCurLineup()!;
        Player = player;
        AeonId = aeonExcel.AeonID;
        AreaExcel = areaExcel;
        AeonExcel = aeonExcel;
        DiceInstance = new ChessRogueDiceInstance(this, player.ChessRogueManager!.GetDice(branchId));
        Layers = areaExcel.LayerIDList;
        CurLayer = Layers.First();
        EventManager = new RogueEventManager(player, this);

        RogueType = rogueSubMode == RogueSubModeEnum.ChessRogueNous ? 160 : 130;

        foreach (var difficulty in areaExcel.DifficultyID)
            if (GameData.RogueDLCDifficultyData.TryGetValue(difficulty, out var diff))
                DifficultyExcel.Add(diff);

        var action = new RogueActionInstance
        {
            QueuePosition = CurActionQueuePosition
        };
        action.SetBonus();

        RogueActions.Add(CurActionQueuePosition, action);

        // generate cells
        GenerateLayer();
    }

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
    public int StartCell { get; set; }

    public List<int> Layers { get; set; }
    public int CurLayer { get; set; }
    public RogueDLCChessBoardExcel? CurBoardExcel { get; set; }
    public ChessRogueLevelStatus CurLevelStatus { get; set; } = ChessRogueLevelStatus.ChessRogueLevelProcessing;

    public bool FirstEnterBattle { get; set; } = true;
    public int LayerMap { get; set; }

    public int ActionPoint { get; set; } = 15;

    public List<int> DisableAeonIds { get; set; } = [];

    public override void OnBattleStart(BattleInstance battle)
    {
        base.OnBattleStart(battle);

        if (DifficultyExcel.Count > 0) battle.CustomLevel = DifficultyExcel.RandomElement().LevelList.RandomElement();

        if (ActionPoint < 0)
        {
            GameData.ActionPointOverdrawData.TryGetValue(Math.Max(ActionPoint, -50), out var overdrawData);
            if (overdrawData != null)
                battle.Buffs.Add(new MazeBuff(overdrawData.MazeBuff, 1, -1)
                {
                    WaveFlag = -1
                });
        }

        CalculateDifficulty(battle);

        if (CurCell!.CellType == RogueDLCBlockTypeEnum.MonsterNousBoss ||
            CurCell!.CellType == RogueDLCBlockTypeEnum.MonsterSwarmBoss)
        {
            var buffList = new List<int>();
            foreach (var buff in BossBuff)
                if (buff.EffectType == BossDecayEffectTypeEnum.AddMazeBuffList)
                    buffList.SafeAddRange(buff.EffectParamList); // add buff
                else
                    foreach (var param in buff.EffectParamList) // remove buff
                        buffList.Remove(param);

            foreach (var buffId in buffList)
                battle.Buffs.Add(new MazeBuff(buffId, 1, -1)
                {
                    WaveFlag = -1
                });
        }
    }

    public void CalculateDifficulty(BattleInstance battle)
    {
        if (DifficultyExcel.Count > 0)
        {
            List<int> difficultyIds = [];
            List<MazeBuff> buffs = [];

            foreach (var difficulty in DifficultyLevel) difficultyIds.Add(difficulty.DifficultyID);

            if (difficultyIds.Contains(103))
                buffs.Add(new MazeBuff(610121, 1, -1)
                {
                    WaveFlag = -1
                });

            if (difficultyIds.Contains(105))
                buffs.Add(new MazeBuff(610121, 1, -1)
                {
                    WaveFlag = -1
                });

            var attributeBuffId = 0;

            if (difficultyIds.Contains(106))
                attributeBuffId = 650204;
            else if (difficultyIds.Contains(104))
                attributeBuffId = 650203;
            else if (difficultyIds.Contains(102))
                attributeBuffId = 650202;
            else if (difficultyIds.Contains(101)) attributeBuffId = 650201;

            buffs.Add(new MazeBuff(attributeBuffId, 1, -1)
            {
                WaveFlag = -1,
                DynamicValues = { { "_RogueLayer", Layers.IndexOf(CurLayer) + 1 } }
            });

            foreach (var buff in buffs) battle.Buffs.Add(buff);
        }
    }

    public override async ValueTask OnBattleEnd(BattleInstance battle, PVEBattleResultCsReq req)
    {
        foreach (var miracle in RogueMiracles.Values) miracle.OnEndBattle(battle);

        if (req.EndStatus != BattleEndStatus.BattleEndWin)
        {
            // quit
            CurLevelStatus = ChessRogueLevelStatus.ChessRogueLevelFailed;
            await Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));
            return;
        }

        await RollBuff(battle.Stages.Count);

        switch (CurCell!.CellType)
        {
            case RogueDLCBlockTypeEnum.MonsterBoss:
                await Player.SendPacket(new PacketChessRogueLayerAccountInfoNotify(this));
                break;
            case RogueDLCBlockTypeEnum.MonsterNousBoss:
            case RogueDLCBlockTypeEnum.MonsterSwarmBoss:
                CurLevelStatus = ChessRogueLevelStatus.ChessRogueLevelFinish;
                await Player.SendPacket(new PacketChessRogueLayerAccountInfoNotify(this));
                await Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));
                break;
        }
    }

    #region Buff Management

    public override async ValueTask RollBuff(int amount)
    {
        if (CurCell!.CellType == RogueDLCBlockTypeEnum.MonsterBoss)
        {
            await RollBuff(amount, 100003, 2); // boss room
            await RollMiracle(1);
        }
        else
        {
            await RollBuff(amount, 100005);
        }
    }

    public async ValueTask AddAeonBuff() // TODO: support for aeon cross
    {
        if (AeonBuffPending) return;
        if (CurAeonBuffCount + CurAeonEnhanceCount >= 4)
            // max aeon buff count
            return;
        var curAeonBuffCount = 0; // current path buff count
        var hintId = AeonId * 100 + 1;
        var enhanceData = GameData.RogueAeonEnhanceData[AeonId];
        var buffData = GameData.RogueAeonBuffData[AeonId];
        foreach (var buff in RogueBuffs)
            if (buff.BuffExcel.RogueBuffType == AeonExcel.RogueBuffType)
            {
                if (!buff.BuffExcel.IsAeonBuff)
                {
                    curAeonBuffCount++;
                }
                else
                {
                    hintId++; // next hint
                    enhanceData.Remove(buff.BuffExcel);
                }
            }

        var needAeonBuffCount = (CurAeonBuffCount + CurAeonEnhanceCount) switch
        {
            0 => 3,
            1 => 6,
            2 => 10,
            3 => 14,
            _ => 100
        };

        if (curAeonBuffCount >= needAeonBuffCount)
        {
            RogueBuffSelectMenu menu = new(this)
            {
                QueueAppend = 2,
                HintId = hintId,
                RollMaxCount = 0,
                RollFreeCount = 0,
                IsAeonBuff = true
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
            await UpdateMenu();
        }
    }

    public override async ValueTask UpdateMenu()
    {
        await base.UpdateMenu();


        await AddAeonBuff(); // check if aeon buff can be added
    }

    #endregion

    #region Map Management

    public async ValueTask EnterNextLayer()
    {
        HistoryCell.Clear();
        CurLayer = Layers[Layers.IndexOf(CurLayer) + 1];
        CurLevelStatus = ChessRogueLevelStatus.ChessRogueLevelProcessing;

        GenerateLayer();
        await EnterCell(StartCell);

        await Player.SendPacket(new PacketChessRogueEnterNextLayerScRsp(this));
    }

    public void GenerateLayer()
    {
        var level = Layers.IndexOf(CurLayer) + 1;
        FirstEnterBattle = true;

        LayerMap = GameConstants.AllowedChessRogueEntranceId.RandomElement();

        CurBoardExcel = RogueSubMode == RogueSubModeEnum.ChessRogue
            ? GameData.RogueSwarmChessBoardData[level].RandomElement()
            : GameData.RogueNousChessBoardData[level].RandomElement();

        RogueCells.Clear();
        CurCell = null;

        StartCell = CurBoardExcel.MapInfo!.StartGridItemID;

        foreach (var item in CurBoardExcel.MapInfo!.RogueChestGridItemMap)
        {
            var cell = new ChessRogueCellInstance(this, item.Value)
            {
                PosY = item.Value.PosY,
                PosX = item.Value.PosX,
                CellId = item.Key
            };
            RogueCells.Add(item.Key, cell);

            //if (cell.GetCellId() == CurBoardExcel.MapInfo!.EndGridItemID)  // last cell
            //{
            cell.Init();
            //}
        }
    }

    public async ValueTask EnterCell(int id, int monsterId = 0)
    {
        if (RogueCells.TryGetValue(id, out var cell))
        {
            if (monsterId > 0)
            {
                cell.SelectMonsterId = monsterId;
                if (BossAeonId == 0) BossAeonId = cell.GetBossAeonId(monsterId);

                var excel = cell.GetBossDecayExcel(monsterId);
                if (excel != null) BossBuff.Add(excel);
            }

            CurCell = cell;
            cell.CellStatus = ChessRogueBoardCellStatus.Finish;

            await Player.EnterMissionScene(cell.GetEntryId(), cell.RoomConfig!.AnchorGroup, cell.RoomConfig!.AnchorId,
                false);

            HistoryCell.Add(cell);

            DiceInstance.CurSurfaceId = 0;
            DiceInstance.DiceStatus = ChessRogueDiceStatus.ChessRogueDiceIdle;
            await Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
        }
    }

    public async ValueTask SelectCell(int cellId)
    {
        var cell = RogueCells[cellId];
        if (cell.CellStatus == ChessRogueBoardCellStatus.Finish) return;

        cell.CellStatus = ChessRogueBoardCellStatus.Selected;

        await Player.SendPacket(new PacketChessRogueCellUpdateNotify(cell, CurBoardExcel?.ChessBoardID ?? 0));
        await CostActionPoint(1);

        await Player.SendPacket(new PacketChessRogueSelectCellScRsp(cellId));
    }

    #endregion

    #region Dice Management

    public async ValueTask RollDice()
    {
        DiceInstance.RollDice();

        await Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
        await Player.SendPacket(new PacketChessRogueRollDiceScRsp(DiceInstance));
    }

    public async ValueTask ReRollDice()
    {
        DiceInstance.RerollTimes--;
        DiceInstance.RollDice();

        await Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
        await Player.SendPacket(new PacketChessRogueReRollDiceScRsp(DiceInstance));
    }

    public async ValueTask CheatDice(int surfaceId)
    {
        DiceInstance.CheatTimes--;
        DiceInstance.CurSurfaceId = surfaceId;

        await Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
        await Player.SendPacket(new PacketChessRogueCheatRollScRsp(DiceInstance, surfaceId));
    }

    public async ValueTask ConfirmRoll()
    {
        DiceInstance.DiceStatus = ChessRogueDiceStatus.ChessRogueDiceConfirmed;

        await Player.SendPacket(new PacketChessRogueUpdateDiceInfoScNotify(DiceInstance));
        await Player.SendPacket(new PacketChessRogueConfirmRollScRsp(DiceInstance));

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

    public async ValueTask CostActionPoint(int cost)
    {
        ActionPoint -= cost;

        await Player.SendPacket(new PacketChessRogueUpdateActionPointScNotify(ActionPoint));
    }

    public async ValueTask QuitRogue()
    {
        CurLevelStatus = ChessRogueLevelStatus.ChessRogueLevelFinish;
        await Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));

        Player.ChessRogueManager!.RogueInstance = null;
        await Player.EnterScene(801120102, 0, false);
        Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);

        await Player.SendPacket(new PacketChessRogueQuitScRsp(this));
    }

    public async ValueTask LeaveRogue()
    {
        CurLevelStatus = ChessRogueLevelStatus.ChessRogueLevelFinish;
        await Player.SendPacket(new PacketChessRogueUpdateLevelBaseInfoScNotify(CurLevelStatus));

        Player.ChessRogueManager!.RogueInstance = null;
        await Player.EnterScene(801120102, 0, false);
        Player.LineupManager!.SetExtraLineup(ExtraLineupType.LineupNone, []);

        await Player.SendPacket(new PacketChessRogueLeaveScRsp(this));
    }

    #endregion

    #region Serialization

    public ChessRogueCurrentInfo ToProto()
    {
        var proto = new ChessRogueCurrentInfo
        {
            GameMiracleInfo = ToMiracleInfo(),
            RogueBuffInfo = ToBuffInfo(),
            RogueAeonInfo = ToAeonInfo(),
            RogueSubMode = (uint)RogueSubMode,
            RogueDiceInfo = DiceInstance.ToProto(),
            RogueLineupInfo = ToLineupInfo(),
            RogueDifficultyInfo = ToDifficultyInfo(),
            VirtualItemInfo = ToVirtualItemInfo(),
            LevelInfo = ToLevelInfo(),
            PendingAction = RogueActions.Count > 0
                ? RogueActions.First().Value.ToProto()
                : new RogueCommonPendingAction()
        };

        proto.RogueCurrentGameInfo.AddRange(ToGameInfo());

        return proto;
    }

    public ChessRoguePlayerInfo ToPlayerProto()
    {
        var playerInfo = new ChessRoguePlayerInfo
        {
            Lineup = Player.LineupManager!.GetCurLineup()!.ToProto(),
            Scene = Player.SceneInstance!.ToProto()
        };

        return playerInfo;
    }

    public ChessRogueQueryGameInfo ToRogueGameInfo()
    {
        var proto = new ChessRogueQueryGameInfo
        {
            RogueSubMode = (uint)RogueSubMode
        };

        proto.RogueCurrentGameInfo.AddRange(ToGameInfo());

        return proto;
    }

    public ChessRogueMiracleInfo ToMiracleInfo()
    {
        var proto = new ChessRogueMiracleInfo
        {
            ChessRogueMiracleInfo_ = new ChessRogueMiracle()
        };

        proto.ChessRogueMiracleInfo_.MiracleList.AddRange(RogueMiracles.Select(x => x.Value.ToGameMiracleProto())
            .ToList());

        return proto;
    }

    public ChessRogueBuffInfo ToBuffInfo()
    {
        var proto = new ChessRogueBuffInfo
        {
            ChessRogueBuffInfo_ = new ChessRogueBuff()
        };

        proto.ChessRogueBuffInfo_.BuffList.AddRange(RogueBuffs.Select(x => x.ToCommonProto()).ToList());

        return proto;
    }

    public ChessRogueAeonInfo ToAeonInfo()
    {
        var proto = new ChessRogueAeonInfo
        {
            AeonId = (uint)AeonId
        };

        return proto;
    }

    public ChessRogueGameAeonInfo ToGameAeonInfo()
    {
        var proto = new ChessRogueGameAeonInfo
        {
            AeonId = (uint)AeonId
        };

        return proto;
    }

    public ChessRogueLineupInfo ToLineupInfo()
    {
        var proto = new ChessRogueLineupInfo
        {
            ReviveInfo = new RogueAvatarReviveCost
            {
                RogueReviveCost = new ItemCostData
                {
                    ItemList =
                    {
                        new ItemCost
                        {
                            PileItem = new PileItem
                            {
                                ItemId = 31,
                                ItemNum = (uint)CurReviveCost
                            }
                        }
                    }
                }
            }
        };

        foreach (var avatar in CurLineup!.BaseAvatars!)
            proto.ChessAvatarList.Add(new ChessRogueLineupAvatarInfo
            {
                AvatarId = (uint)avatar.BaseAvatarId
            });

        return proto;
    }

    public List<RogueGameInfo> ToGameInfo()
    {
        var proto = new List<RogueGameInfo>
        {
            new()
            {
                RogueDifficultyInfo = ToDifficultyLevelInfo()
            },
            new()
            {
                RogueAeonInfo = ToGameAeonInfo()
            },
            new(),
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

        foreach (var level in DifficultyLevel) proto.DifficultyIdList.Add((uint)level.DifficultyID);

        return proto;
    }

    public RogueDifficultyLevelInfo ToDifficultyLevelInfo()
    {
        var proto = new RogueDifficultyLevelInfo();

        foreach (var level in DifficultyLevel) proto.DifficultyIdList.Add((uint)level.DifficultyID);

        return proto;
    }

    public RogueVirtualItem ToVirtualItemInfo()
    {
        return new RogueVirtualItem
        {
            RogueMoney = (uint)CurMoney
        };
    }

    public ChessRogueLevelInfo ToLevelInfo()
    {
        List<uint> canSelected = [];
        foreach (var cell in RogueCells.Where(cell => cell.Value.CellStatus == ChessRogueBoardCellStatus.Idle))
        {
            if (cell.Value.PosY == CurCell!.PosY - 1 || cell.Value.PosY == CurCell!.PosY + 1)
                if (cell.Value.PosX == CurCell!.PosX || cell.Value.PosX == CurCell!.PosX + 1)
                    canSelected.Add((uint)cell.Value.GetCellId());
            if (cell.Value.PosY != CurCell!.PosY) continue;
            if (cell.Value.PosX == CurCell!.PosX + 2)
                canSelected.Add((uint)cell.Value.GetCellId());
        }

        var proto = new ChessRogueLevelInfo
        {
            LevelStatus = (uint)CurLevelStatus,
            ActionPoint = ActionPoint,
            Id = (uint)AreaExcel.AreaID,
            LayerId = (uint)CurLayer,
            AreaInfo = new ChessRogueAreaInfo
            {
                LayerStatus = ChessRogueBoardCellStatus.Processing,
                CurId = (uint)CurCell!.GetCellId(),
                CurBoardId = (uint)(CurBoardExcel?.ChessBoardID ?? 0),
                Cell = new CellInfo
                {
                    CellList = { RogueCells.Select(x => x.Value.ToProto()).ToList() }
                },
                AllowedSelectCellIdList = { canSelected },
                HistoryCell = { HistoryCell.Select(x => x.ToHistoryProto()).ToList() }
            }
        };

        return proto;
    }

    public ChessRogueFinishInfo ToFinishInfo()
    {
        var info = new ChessRogueFinishInfo
        {
            EndAreaId = (uint)AreaExcel.AreaID,
            LastLayerId = (uint)CurLayer,
            RogueLineup = CurLineup!.ToProto(),
            DifficultyLevel =
                uint.Parse(AreaExcel.AreaID.ToString().Substring(AreaExcel.AreaID.ToString().Length - 1, 1)),
            RogueSubMode = (uint)RogueSubMode,
            RogueBuffInfo = new ChessRogueBuff
            {
                BuffList = { RogueBuffs.Select(x => x.ToCommonProto()) }
            },
            GameMiracleInfo = new ChessRogueMiracle
            {
                MiracleList = { RogueMiracles.Select(x => x.Value.ToGameMiracleProto()) }
            }
        };

        return info;
    }

    #endregion
}