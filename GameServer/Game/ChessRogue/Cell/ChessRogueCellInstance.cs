using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using CellFinalMonsterInfo = EggLink.DanhengServer.Proto.CellFinalMonsterInfo;

namespace EggLink.DanhengServer.GameServer.Game.ChessRogue.Cell;

public class ChessRogueCellInstance
{
    public ChessRogueCellInstance(ChessRogueInstance instance, RogueChestGridItem item)
    {
        Instance = instance;
        Layer = instance.Layers.IndexOf(instance.CurLayer) + 1;
        var list = new RandomList<RogueDLCBlockTypeEnum>();
        list.Add(RogueDLCBlockTypeEnum.MonsterNormal, 8);
        list.Add(RogueDLCBlockTypeEnum.Reward, 4);
        list.Add(RogueDLCBlockTypeEnum.Event, 6);
        list.Add(RogueDLCBlockTypeEnum.NousSpecialEvent, 4);
        list.Add(RogueDLCBlockTypeEnum.NousEvent, 2);

        BlockType = item.BlockTypeList.Count > 0 ? item.BlockTypeList.RandomElement() : list.GetRandom();
    }

    public RogueDLCBlockTypeEnum BlockType { get; set; }
    public int PosY { get; set; }
    public int PosX { get; set; }
    public int CellId { get; set; }
    public int RoomId { get; set; }
    public int Layer { get; set; }
    public ChessRogueInstance Instance { get; set; }
    public ChessRogueBoardCellStatus CellStatus { get; set; } = ChessRogueBoardCellStatus.Idle;
    public ChessRogueRoomConfig? RoomConfig { get; set; }
    public RogueCellMarkTypeEnum MarkType { get; set; } = RogueCellMarkTypeEnum.None;
    public int SelectMonsterId { get; set; }

    public List<int> SelectedDecayId { get; set; } = [];

    public List<ChessRogueCellAdvanceInfo> CellAdvanceInfo { get; set; } = [];

    public void Init()
    {
        switch (BlockType)
        {
            // boss
            case RogueDLCBlockTypeEnum.MonsterBoss when Layer == 1:
            {
                var randomList = Enumerable.Range(101, 7).Where(i => i != 103).ToList();

                var random1 = randomList.RandomElement();
                randomList.Remove(random1);
                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo
                {
                    BossDecayId = random1,
                    MonsterId = Random.Shared.Next(221001, 221018)
                });

                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo
                {
                    BossDecayId = randomList.RandomElement(),
                    MonsterId = Random.Shared.Next(221001, 221018)
                });
                break;
            }
            case RogueDLCBlockTypeEnum.MonsterBoss:
            {
                var randomList = Enumerable.Range(2, 3).Select(i => i * 10 + 101).ToList();

                var random1 = randomList.RandomElement();
                randomList.Remove(random1);

                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo
                {
                    BossDecayId = random1,
                    MonsterId = Random.Shared.Next(222001, 222007)
                });

                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo
                {
                    BossDecayId = randomList.RandomElement(),
                    MonsterId = Random.Shared.Next(222001, 222017)
                });
                break;
            }
            case RogueDLCBlockTypeEnum.MonsterNousBoss:
            case RogueDLCBlockTypeEnum.MonsterSwarmBoss:
            {
                // last boss
                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo
                {
                    BossDecayId = 114,
                    MonsterId = 223003
                });
                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo
                {
                    BossDecayId = 102 + Instance.BossAeonId * 10,
                    MonsterId = Random.Shared.Next(223001, 223003)
                });

                foreach (var decay in Instance.BossBuff) SelectedDecayId.Add(decay.BossDecayID);
                break;
            }
        }
    }

    public int GetBossAeonId(int monsterId)
    {
        var info = CellAdvanceInfo.Find(x => x.MonsterId == monsterId);

        return info != null ? Math.Max((info.BossDecayId - 101) / 10, 0) : 0;
    }

    public RogueDLCBossDecayExcel? GetBossDecayExcel(int monsterId)
    {
        var info = CellAdvanceInfo.Find(x => x.MonsterId == monsterId);
        if (info == null) return null;
        GameData.RogueDLCBossDecayData.TryGetValue(info.BossDecayId, out var excel);

        return excel;
    }

    public int GetCellId()
    {
        return CellId;
    }

    public bool IsCollapsed()
    {
        var curCell = Instance.CurCell;
        if (curCell == null) return true;

        return curCell.PosX >= PosX; // only check the left side of the board
    }

    public int GetEntryId()
    {
        if (RoomConfig != null) return RoomConfig.EntranceId;
        var pool = GameData.ChessRogueRoomData[BlockType].FindAll(x => x.EntranceId == Instance.LayerMap).ToList();
        RoomConfig = pool.RandomElement();
        if (Instance.FirstEnterBattle && BlockType == RogueDLCBlockTypeEnum.MonsterNormal)
        {
            do
            {
                RoomConfig = pool.RandomElement();
            } while (RoomConfig.SubMonsterGroup.Count == 0); // make sure the room has sub monster

            Instance.FirstEnterBattle = false;
        }

        RoomId = RoomConfig.RoomPrefix * 10000 + (int)BlockType * 100 +
                 Random.Shared.Next(1, 10); // find a better way to generate room id

        return RoomConfig.EntranceId;
    }

    public int GetRow()
    {
        return PosX;
    }

    public List<int> GetLoadGroupList()
    {
        var groupList = new List<int>();
        groupList.AddRange(RoomConfig!.DefaultLoadBasicGroup);

        switch (MarkType)
        {
            case RogueCellMarkTypeEnum.Choice:
                groupList.AddRange(RoomConfig.SelectEventLoadGroup);
                break;
            case RogueCellMarkTypeEnum.Double:
                groupList.AddRange(RoomConfig.DoubleEventLoadGroup);
                break;
            default:
                groupList.AddRange(RoomConfig.DefaultLoadGroup);
                break;
        }

        groupList.AddRange(RoomConfig.SubMonsterGroup);

        return groupList;
    }

    public ChessRogueCell ToProto()
    {
        var info = new ChessRogueCell
        {
            CellStatus = CellStatus,
            PosY = (uint)PosY,
            Id = (uint)GetCellId(),
            BlockType = (uint)BlockType,
            IsUnlock = true,
            RoomId = (uint)RoomId,
            IsUnlocked = true,
            PosX = (uint)GetRow(),
            MarkType = (uint)MarkType
        };

        if (CellAdvanceInfo.Count <= 0) return info;
        if (SelectedDecayId.Count > 0)
            info.StageInfo = new CellAdvanceInfo
            {
                FinalBossInfo = new CellFinalMonsterInfo
                {
                    CellBossInfo = new CellMonsterInfo
                    {
                        CellMonsterList = { CellAdvanceInfo.Select(x => x.ToProto()).ToList() },
                        SelectBossId = (uint)SelectMonsterId
                    },
                    SelectBossInfo = new CellMonsterSelectInfo
                    {
                        SelectDecayId = { SelectedDecayId.Select(x => (uint)x) }
                    }
                }
            };
        else
            info.StageInfo = new CellAdvanceInfo
            {
                CellBossInfo = new CellMonsterInfo
                {
                    CellMonsterList = { CellAdvanceInfo.Select(x => x.ToProto()).ToList() },
                    SelectBossId = (uint)SelectMonsterId
                }
            };

        return info;
    }

    public ChessRogueHistoryCellInfo ToHistoryProto()
    {
        var info = new ChessRogueHistoryCellInfo
        {
            RoomId = (uint)RoomId,
            CellId = (uint)GetCellId()
        };

        return info;
    }
}

public class ChessRogueCellAdvanceInfo
{
    public int BossDecayId { get; set; }
    public int MonsterId { get; set; }

    public CellMonster ToProto()
    {
        return new CellMonster
        {
            BossDecayId = (uint)BossDecayId,
            MonsterId = (uint)MonsterId
        };
    }
}