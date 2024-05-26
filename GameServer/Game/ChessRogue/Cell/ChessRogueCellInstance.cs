using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.ChessRogue.Cell
{
    public class ChessRogueCellInstance
    {
        public int CellType { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int RoomId { get; set; }
        public int Layer { get; set; } = 1;
        public int MapId { get; set; }
        public ChessRogueInstance Instance { get; set; }
        public ChessRogueBoardCellStatus CellStatus { get; set; } = ChessRogueBoardCellStatus.Idle;
        public ChessRogueRoomConfig? RoomConfig { get; set; }
        public ChessRogueCellConfig? CellConfig { get; set; }
        public int SelectMonsterId { get; set; }

        public List<int> SelectedDecayId { get; set; } = [];

        public List<ChessRogueCellAdvanceInfo> CellAdvanceInfo { get; set; } = [];
        
        public ChessRogueCellInstance(ChessRogueInstance instance)
        {
            Instance = instance;
            Layer = instance.Layers.IndexOf(instance.CurLayer) + 1;

            var list = new RandomList<int>();
            list.Add(3, 8);
            list.Add(7, 4);
            list.Add(8, 6);
            list.Add(17, 4);
            list.Add(16, 2);

            CellType = list.GetRandom();
        }

        public void Init()
        {
            if (CellType == 11)
            {
                // boss
                if (Layer == 1)
                {
                    var randomList = new List<int>();
                    foreach (var i in Enumerable.Range(101, 7))
                    {
                        if (i == 103) continue;
                        randomList.Add(i);
                    }

                    var random1 = randomList.RandomElement();
                    randomList.Remove(random1);
                    CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo()
                    {
                        BossDecayId = random1,
                        MonsterId = Random.Shared.Next(221001, 221018),
                    });

                    CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo()
                    {
                        BossDecayId = randomList.RandomElement(),
                        MonsterId = Random.Shared.Next(221001, 221018),
                    });
                }
                else
                {
                    var randomList = new List<int>();
                    foreach (var i in Enumerable.Range(2, 3))
                    {
                        randomList.Add(i * 10 + 101);
                    }

                    var random1 = randomList.RandomElement();
                    randomList.Remove(random1);

                    CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo()
                    {
                        BossDecayId = random1,
                        MonsterId = Random.Shared.Next(222001, 222007)
                    });

                    CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo()
                    {
                        BossDecayId = randomList.RandomElement(),
                        MonsterId = Random.Shared.Next(222001, 222017)
                    });
                }
            }
            else if (CellType == 15)
            {
                // last boss
                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo()
                {
                    BossDecayId = 114,
                    MonsterId = 223003
                });
                CellAdvanceInfo.Add(new ChessRogueCellAdvanceInfo()
                {
                    BossDecayId = 102 + Instance.BossAeonId * 10,
                    MonsterId = Random.Shared.Next(223001, 223003)
                });

                foreach (var decay in Instance.BossBuff)
                {
                    SelectedDecayId.Add(decay.BossDecayID);
                }
            }
        }

        public int GetBossAeonId(int monsterId)
        {
            var info = CellAdvanceInfo.Find(x => x.MonsterId == monsterId);
            if (info != null)
            {
                return Math.Max((info.BossDecayId - 101 ) / 10, 0);
            }
            return 0;
        }

        public RogueDLCBossDecayExcel? GetBossDecayExcel(int monsterId)
        {
            var info = CellAdvanceInfo.Find(x => x.MonsterId == monsterId);
            if (info != null)
            {
                GameData.RogueDLCBossDecayData.TryGetValue(info.BossDecayId, out var excel);
                return excel;
            }
            return null;
        }

        public int GetCellId() => Column * 100 + Row;

        public int GetEntryId()
        {
            List<int> mapList = [];
            foreach (var cell in GameData.ChessRogueCellGenData)
            {
                var cellType = int.Parse(cell.Key.ToString().Substring(3, 2));
                if (cellType != CellType) continue;

                var mapId = int.Parse(cell.Key.ToString()[..3]);
                mapList.SafeAdd(mapId);
            }

            MapId = mapList.RandomElement();
            RoomConfig = GameData.ChessRogueRoomGenData[MapId];

            var randomList = new List<int>();
            foreach (var key in GameData.ChessRogueCellGenData.Keys)
            {
                if (key.ToString().StartsWith($"{MapId * 100 + CellType}"))
                {
                    randomList.Add(key);
                }
            }

            RoomId = randomList.RandomElement();
            CellConfig = GameData.ChessRogueCellGenData[RoomId];

            return RoomConfig.EntranceId;
        }

        public int GetRow()
        {
            if (Column == 0 || Column == 4 || Column == 2)
            {
                return Row * 2;
            }
            else if (Column == 1 || Column == 3)
            {
                return Row * 2 + 1;
            }
            return Row;
        }

        public List<int> GetLoadGroupList()
        {
            var groupList = new List<int>();
            if (RoomConfig!.CellGroup.TryGetValue(CellType, out ChessRogueRoom? value))
            {
                groupList.AddRange(value.Groups);
            }
            groupList.AddRange(CellConfig?.Groups ?? []);
            groupList.AddRange(RoomConfig.Groups);

            return groupList;
        }

        public ChessRogueCell ToProto()
        {
            var info = new ChessRogueCell()
            {
                CellStatus = CellStatus,
                Column = (uint)Column,
                Id = (uint)GetCellId(),
                CellType = (uint)CellType,
                IsValid = true,
                RoomId = (uint)RoomId,
                KJMDBCKGFAM = true,
                Row = (uint)GetRow(),
            };

            if (CellAdvanceInfo.Count > 0)
            {
                info.AdvanceInfo = new()
                {
                    BossInfo = new()
                    {
                        MonsterList = { CellAdvanceInfo.Select(x => x.ToProto()).ToList() },
                        SelectBossId = (uint)SelectMonsterId
                    },
                };

                if (SelectedDecayId.Count > 0)
                {
                    info.AdvanceInfo.SelectBossInfo = new()
                    {
                        SelectDecayId = { SelectedDecayId.Select(x => (uint)x).ToList() }
                    };
                }
            }

            return info;
        }

        public ChessRogueHistoryCellInfo ToHistoryProto()
        {
            var info = new ChessRogueHistoryCellInfo()
            {
                RoomId = (uint)RoomId,
                CellId = (uint)GetCellId(),
            };

            return info;
        }
    }

    public class ChessRogueCellAdvanceInfo
    {
        public int BossDecayId { get; set; }
        public int MonsterId { get; set; }

        public CellMonster ToProto() => new()
        {
            BossDecayId = (uint)BossDecayId,
            MonsterId = (uint)MonsterId
        };
    }
}
