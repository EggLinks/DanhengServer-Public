using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournFormula.json")]
public class RogueTournFormulaExcel : ExcelResource
{
    public string FormulaIcon { get; set; } = "";
    public string UltraFormulaIcon { get; set; } = "";
    public string FormulaSubIcon { get; set; } = "";
    public string FormulaStoryJson { get; set; } = "";
    public int MazeBuffID { get; set; }
    public int SubBuffNum { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournModeEnum TournMode { get; set; }

    public int UnlockDisplayID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueFormulaCategoryEnum FormulaCategory { get; set; }

    public bool IsInHandbook { get; set; }
    public int MainBuffTypeID { get; set; }
    public int FormulaDisplayID { get; set; }
    public int FormulaID { get; set; }
    public int MainBuffNum { get; set; }
    public int SubBuffTypeID { get; set; }

    public override int GetId()
    {
        return FormulaID;
    }

    public override void Loaded()
    {
        GameData.RogueTournFormulaData.TryAdd(GetId(), this);
    }

    public FormulaInfo ToProto(List<int> buffIdList)
    {
        var proto = new FormulaInfo
        {
            FormulaId = (uint)FormulaID,
            IsExpand = IsExpanded(buffIdList)
        };

        proto.FormulaBuffTypeList.Add(new FormulaBuffTypeInfo
        {
            Key = (uint)MainBuffTypeID,
            FormulaBuffNum = MainBuffNum
        });

        if (SubBuffTypeID != 0)
            proto.FormulaBuffTypeList.Add(new FormulaBuffTypeInfo
            {
                Key = (uint)SubBuffTypeID,
                FormulaBuffNum = SubBuffNum
            });

        return proto;
    }

    public bool IsExpanded(List<int> buffIdList)
    {
        Dictionary<int, int> buffTypeNumDict = new();
        foreach (var buff in buffIdList.Select(buffId => GameData.RogueBuffData.GetValueOrDefault(buffId * 100 + 1))
                     .OfType<RogueTournBuffExcel>()
                     .Where(buff => !buffTypeNumDict.TryAdd(buff.RogueBuffType, 1)))
            buffTypeNumDict[buff.RogueBuffType]++;

        if (buffTypeNumDict.GetValueOrDefault(MainBuffTypeID, 0) < MainBuffNum) return false;

        return SubBuffTypeID == 0 || buffTypeNumDict.GetValueOrDefault(SubBuffTypeID, 0) >= SubBuffNum;
    }

    public RogueCommonActionResult ToResultProto(RogueCommonActionResultSourceType select, List<int> buffIdList)
    {
        return new RogueCommonActionResult
        {
            RogueAction = new RogueCommonActionResultData
            {
                GetFormulaList = new RogueCommonFormula
                {
                    FormulaInfo = ToProto(buffIdList)
                }
            },
            Source = select
        };
    }

    public RogueCommonActionResult ToRemoveResultProto(RogueCommonActionResultSourceType select, List<int> buffIdList)
    {
        return new RogueCommonActionResult
        {
            RogueAction = new RogueCommonActionResultData
            {
                RemoveFormulaList = new RogueCommonRemoveFormula
                {
                    FormulaInfo = ToProto(buffIdList)
                }
            },
            Source = select
        };
    }

    public RogueCommonActionResult ToExpandResultProto(RogueCommonActionResultSourceType select, List<int> buffIdList)
    {
        return new RogueCommonActionResult
        {
            RogueAction = new RogueCommonActionResultData
            {
                ExpandFormulaList = new RogueCommonExpandedFormula
                {
                    FormulaInfo = ToProto(buffIdList)
                }
            },
            Source = select
        };
    }

    public RogueCommonActionResult ToContractResultProto(RogueCommonActionResultSourceType select, List<int> buffIdList)
    {
        return new RogueCommonActionResult
        {
            RogueAction = new RogueCommonActionResultData
            {
                ContractFormulaList = new RogueCommonContractFormula
                {
                    FormulaInfo = ToProto(buffIdList)
                }
            },
            Source = select
        };
    }
}