﻿using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Buff;

public class RogueBuffSelectMenu(BaseRogueInstance rogue)
{
    public int HintId { get; set; } = 1;
    public List<RogueBuffExcel> Buffs { get; set; } = [];
    public int RollMaxCount { get; set; } = rogue.BaseRerollCount;
    public int RollCount { get; set; }
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
            if (buff.RogueBuffType == rogue.RogueBuffType)
                list.Add(buff, (int)(20f / (int)buff.RogueBuffCategory * 2.5));
            else
                list.Add(buff, (int)(20f / (int)buff.RogueBuffCategory * 0.7));
        var result = new List<RogueBuffExcel>();

        for (var i = 0; i < count; i++)
        {
            var buff = list.GetRandom();
            if (buff != null)
            {
                result.Add(buff);
                list.Remove(buff);
            }

            if (list.GetCount() == 0) break; // No more buffs to roll
        }

        Buffs = result;
    }

    public async ValueTask RerollBuff()
    {
        if (RollFreeCount > 0)
        {
            RollFreeCount--; // Free reroll
        }
        else
        {
            if (RollMaxCount - RollCount <= 0) return;
            RollCount++; // Paid reroll
            await rogue.CostMoney(RollCost);
        }

        RollBuff(BuffPool.ToList());
    }

    public RogueActionInstance GetActionInstance()
    {
        rogue.CurActionQueuePosition += QueueAppend;
        return new RogueActionInstance
        {
            QueuePosition = rogue.CurActionQueuePosition,
            RogueBuffSelectMenu = this
        };
    }

    public RogueCommonBuffSelectInfo ToProto()
    {
        return new RogueCommonBuffSelectInfo
        {
            CanRoll = true,
            RollBuffCount = (uint)RollCount,
            RollBuffFreeCount = (uint)RollFreeCount,
            RollBuffMaxCount = (uint)RollMaxCount,
            SourceCurCount = (uint)CurCount,
            SourceTotalCount = (uint)TotalCount,
            RollBuffCostData = new ItemCostData
            {
                ItemList =
                {
                    new ItemCost
                    {
                        PileItem = new PileItem
                        {
                            ItemId = 31,
                            ItemNum = (uint)RollCost
                        }
                    }
                }
            },
            SourceHintId = (uint)HintId,
            HandbookUnlockBuffIdList = { Buffs.Select(x => (uint)x.MazeBuffID) },
            SelectBuffList = { Buffs.Select(x => x.ToProto()) }
        };
    }
}