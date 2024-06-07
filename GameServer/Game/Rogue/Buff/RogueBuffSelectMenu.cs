using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Buff
{
    public class RogueBuffSelectMenu(BaseRogueInstance rogue)
    {
        public int HintId { get; set; } = 1;
        public List<RogueBuffExcel> Buffs { get; set; } = [];
        public int RollMaxCount { get; set; } = rogue.BaseRerollCount;
        public int RollCount { get; set; } = 0;
        public int RollFreeCount { get; set; } = rogue.BaseRerollFreeCount;
        public int RollCost { get; set; } = rogue.CurRerollCost;
        public int QueueAppend { get; set; } = 3;
        public bool IsAeonBuff { get; set; } = false;
        public int CurCount { get; set; } = 1;
        public int TotalCount { get; set; } = 1;
        public List<RogueBuffExcel> BuffPool { get; set; } = [];

        public void RollBuff(List<RogueBuffExcel> buffs, int count = 3)
        {
            BuffPool.Clear();
            BuffPool.AddRange(buffs);

            var list = new RandomList<RogueBuffExcel>();

            foreach (var buff in buffs)
            {
                if (buff.RogueBuffType == rogue.RogueBuffType)
                {
                    list.Add(buff, (int)(20 / buff.RogueBuffRarity * 2.5));
                } else
                {
                    list.Add(buff, (int)(20 / buff.RogueBuffRarity * 0.7));
                }
            }
            var result = new List<RogueBuffExcel>();

            for (var i = 0; i < count; i++)
            {
                var buff = list.GetRandom();
                if (buff != null)
                {
                    result.Add(buff);
                    list.Remove(buff);
                }
                if (list.GetCount() == 0) break;  // No more buffs to roll
            }

            Buffs = result;
        }

        public void RerollBuff()
        {
            if (RollFreeCount > 0)
            {
                RollFreeCount--;  // Free reroll
            } else
            {
                if (RollCount <= 0) return;
                RollCount--;  // Paid reroll
                rogue.CostMoney(RollCost);
            }

            RollBuff(BuffPool);
        }

        public RogueActionInstance GetActionInstance()
        {
            rogue.CurActionQueuePosition += QueueAppend;
            return new()
            {
                QueuePosition = rogue.CurActionQueuePosition,
                RogueBuffSelectMenu = this
            };
        }

        public RogueCommonBuffSelectInfo ToProto()
        {
            return new()
            {
                CanRoll = RollCount > 0,
                RollBuffCount = (uint)RollCount,
                RollBuffFreeCount = (uint)RollFreeCount,
                RollBuffMaxCount = (uint)RollMaxCount,
                SourceCurCount = (uint)CurCount,
                SourceTotalCount = (uint)TotalCount,
                RollBuffCostData = new ItemCostData()
                {
                    ItemList = { new ItemCost()
                    {
                        PileItem = new()
                        {
                            ItemId = 31,
                            ItemNum = (uint)RollCost
                        }
                    } }
                },
                SourceHintId = (uint)HintId,
                HandbookUnlockBuffIdList = { Buffs.Select(x => (uint)x.MazeBuffID) },
                SelectBuffList = { Buffs.Select(x => x.ToProto()) }
            };
        }
    }
}
