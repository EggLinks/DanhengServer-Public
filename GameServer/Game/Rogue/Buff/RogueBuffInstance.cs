using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Buff;

public class RogueBuffInstance(int buffId, int buffLevel)
{
    public int BuffId { get; set; } = buffId;
    public int BuffLevel { get; set; } = buffLevel;
    public BaseRogueBuffExcel BuffExcel { get; set; } = GameData.RogueBuffData[buffId * 100 + buffLevel];

    public int CurSp { get; set; } = 10000;
    public int MaxSp { get; set; } = 10000;

    public int EnhanceCost => 100 + ((int)BuffExcel.RogueBuffCategory - 1) * 30;

    public void OnStartBattle(BattleInstance battle)
    {
        if (BuffExcel is RogueBuffExcel { BattleEventBuffType: RogueBuffAeonTypeEnum.BattleEventBuff })
        {
            GameData.RogueBattleEventData.TryGetValue(BuffExcel.RogueBuffType, out var battleEvent);
            if (battleEvent == null) return;
            battle.BattleEvents.Add(BuffId, new BattleEventInstance(battleEvent.BattleEventID, CurSp, MaxSp));
        }

        battle.Buffs.Add(new MazeBuff(BuffId, BuffLevel, -1)
        {
            WaveFlag = -1
        });
    }

    public RogueBuff ToProto()
    {
        return new RogueBuff
        {
            BuffId = (uint)BuffId,
            Level = (uint)BuffLevel
        };
    }

    public RogueCommonBuff ToCommonProto()
    {
        return new RogueCommonBuff
        {
            BuffId = (uint)BuffId,
            BuffLevel = (uint)BuffLevel
        };
    }

    public RogueCommonActionResult ToResultProto(RogueCommonActionResultSourceType source)
    {
        return new RogueCommonActionResult
        {
            RogueAction = new RogueCommonActionResultData
            {
                GetBuffList = new RogueCommonBuff
                {
                    BuffId = (uint)BuffId,
                    BuffLevel = (uint)BuffLevel
                }
            },
            Source = source
        };
    }

    public RogueCommonActionResult ToRemoveResultProto(RogueCommonActionResultSourceType source)
    {
        return new RogueCommonActionResult
        {
            RogueAction = new RogueCommonActionResultData
            {
                RemoveBuffList = new RogueCommonBuff
                {
                    BuffId = (uint)BuffId,
                    BuffLevel = (uint)BuffLevel
                }
            },
            Source = source
        };
    }

    public RogueBuffEnhanceInfo ToEnhanceProto()
    {
        return new RogueBuffEnhanceInfo
        {
            BuffId = (uint)BuffId,
            CostData = new ItemCostData
            {
                ItemList =
                {
                    new ItemCost
                    {
                        PileItem = new PileItem
                        {
                            ItemId = 31,
                            ItemNum = (uint)EnhanceCost
                        }
                    }
                }
            }
        };
    }

    public ChessRogueBuffEnhanceInfo ToChessEnhanceProto()
    {
        return new ChessRogueBuffEnhanceInfo
        {
            BuffId = (uint)BuffId,
            CostData = new ItemCostData
            {
                ItemList =
                {
                    new ItemCost
                    {
                        PileItem = new PileItem
                        {
                            ItemId = 31,
                            ItemNum = (uint)EnhanceCost
                        }
                    }
                }
            }
        };
    }
}