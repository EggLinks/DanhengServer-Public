using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;

public class RogueWorkbenchProp(SceneInstance scene, MazePropExcel excel, GroupInfo group, PropInfo prop)
    : RogueProp(scene, excel, group, prop)
{
    public RogueWorkbenchProp(EntityProp prop) : this(prop.Scene, prop.Excel, prop.Group, prop.PropInfo)
    {
    }

    public int WorkbenchId { get; set; }
    public List<RogueWorkbenchFunc> WorkbenchFuncs { get; set; } = [];

    public override SceneEntityInfo ToProto()
    {
        var proto = base.ToProto();

        if (WorkbenchId != 0)
            proto.Prop.ExtraInfo = new PropExtraInfo
            {
                RogueTournWorkbenchInfo = new RogueTournWorkbenchInfo
                {
                    WorkbenchId = (uint)WorkbenchId,
                    WorkbenchFuncList = { WorkbenchFuncs.Select(x => x.ToIdInfo()) }
                }
            };

        return proto;
    }
}

public class RogueWorkbenchFunc(RogueTournWorkbenchFuncExcel excel)
{
    public int FuncId { get; set; } = excel.FuncID;
    public RogueTournWorkbenchFuncExcel Excel { get; set; } = excel;

    public int CurNum { get; set; } = excel.FuncType switch
    {
        RogueTournWorkbenchFuncTypeEnum.BuffReforge => -1, // infinite
        RogueTournWorkbenchFuncTypeEnum.FormulaReforge => 5,
        RogueTournWorkbenchFuncTypeEnum.BuffEnhance => 5,
        _ => 0
    };

    public int MaxNum { get; set; } = 5;

    public int CurCost { get; set; } = excel.FuncType switch
    {
        RogueTournWorkbenchFuncTypeEnum.BuffReforge => 0,
        RogueTournWorkbenchFuncTypeEnum.FormulaReforge => 50,
        _ => 0
    };

    public int CurFreeNum { get; set; } = excel.FuncType switch
    {
        RogueTournWorkbenchFuncTypeEnum.BuffReforge => 1,
        RogueTournWorkbenchFuncTypeEnum.FormulaReforge => 0,
        _ => 0
    };

    public WorkbenchFuncIdInfo ToIdInfo()
    {
        return new WorkbenchFuncIdInfo
        {
            WorkbenchFuncId = (uint)FuncId,
            IsValid = true
        };
    }

    public WorkbenchFuncInfo ToProto()
    {
        var proto = new WorkbenchFuncInfo();

        switch (Excel.FuncType)
        {
            case RogueTournWorkbenchFuncTypeEnum.BuffEnhance:
                proto.EnhanceBuffFunc = new WorkbenchEnhanceBuffFuncInfo
                {
                    CurNum = (uint)CurNum,
                    MaxNum = (uint)MaxNum,
                    BuffEnhanceCostMap =
                    {
                        { 1, 1 },
                        { 2, 2 },
                        { 3, 3 }
                    }
                };
                break;
            case RogueTournWorkbenchFuncTypeEnum.BuffReforge:
                proto.ReforgeBuffFunc = new WorkbenchReforgeBuffFuncInfo
                {
                    CostData = new ItemCostData
                    {
                        ItemList =
                        {
                            new ItemCost
                            {
                                PileItem = new PileItem
                                {
                                    ItemId = 31,
                                    ItemNum = (uint)CurCost
                                }
                            }
                        }
                    },
                    CanFreeReforge = CurFreeNum > 0,
                    FreeReforgeNum = (uint)CurFreeNum,
                    IntReforgeNumValue = CurNum,
                    UintReforgeNumValue = (uint)CurNum
                };
                break;
            case RogueTournWorkbenchFuncTypeEnum.FormulaReforge:
                proto.ReforgeFormulaFunc = new WorkbenchReforgeFormulaFuncInfo
                {
                    CostData = new ItemCostData
                    {
                        ItemList =
                        {
                            new ItemCost
                            {
                                PileItem = new PileItem
                                {
                                    ItemId = 31,
                                    ItemNum = (uint)CurCost
                                }
                            }
                        }
                    },
                    CanFreeReforge = CurFreeNum > 0,
                    FreeReforgeNum = (uint)CurFreeNum,
                    IntReforgeNumValue = CurNum,
                    UintReforgeNumValue = (uint)CurNum
                };
                break;
        }

        return proto;
    }
}