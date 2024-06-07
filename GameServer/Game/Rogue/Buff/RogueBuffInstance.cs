using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Buff
{
    public class RogueBuffInstance(int buffId, int buffLevel)
    {
        public int BuffId { get; set; } = buffId;
        public int BuffLevel { get; set; } = buffLevel;
        public RogueBuffExcel BuffExcel { get; set; } = GameData.RogueBuffData[buffId * 100 + buffLevel];

        public int CurSp { get; set; } = 10000;
        public int MaxSp { get; set; } = 10000;

        public void OnStartBattle(BattleInstance battle)
        {
            if (BuffExcel.BattleEventBuffType == Enums.Rogue.RogueBuffAeonTypeEnum.BattleEventBuff)
            {
                GameData.RogueBattleEventData.TryGetValue(BuffExcel.RogueBuffType, out var battleEvent);
                if (battleEvent == null) return;
                battle.BattleEvents.Add(BuffId, new(battleEvent.BattleEventID, CurSp, MaxSp));
            }
            battle.Buffs.Add(new(BuffId, BuffLevel, -1)
            {
                WaveFlag = -1
            });
        }

        public int EnhanceCost => 100 + ((BuffExcel.RogueBuffRarity - 1) * 30);

        public RogueBuff ToProto() => new()
        {
            BuffId = (uint)BuffId,
            Level = (uint)BuffLevel
        };

        public RogueCommonBuff ToCommonProto() => new()
        {
            BuffId = (uint)BuffId,
            BuffLevel = (uint)BuffLevel
        };

        public RogueCommonActionResult ToResultProto(RogueActionSource source) => new()
        {
            RogueAction = new()
            {
                GetBuffList = new()
                {
                    BuffId = (uint)BuffId,
                    BuffLevel = (uint)BuffLevel
                }
            },
            Source = source
        };

        public RogueBuffEnhance ToEnhanceProto() => new()
        {
            BuffId = (uint)BuffId,
            CostData = new()
            {
                ItemList = { new ItemCost() 
                {
                    PileItem = new()
                    {
                        ItemId = 31,
                        ItemNum = (uint)EnhanceCost
                    }
                } }
            }
        };

        public ChessRogueBuffEnhance ToChessEnhanceProto() => new()
        {
            BuffId = (uint)BuffId,
            CostData = new()
            {
                ItemList = { new ItemCost() 
                {
                    PileItem = new()
                    {
                        ItemId = 31,
                        ItemNum = (uint)EnhanceCost
                    }
                } }
            }
        };
    }
}
