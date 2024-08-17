using EggLink.DanhengServer.Enums.TournRogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournFormula.json")]
public class RogueTournFormulaExcel : ExcelResource
{
    public string FormulaIcon{ get; set; }
    public string UltraFormulaIcon { get; set; }
    public string FormulaSubIcon { get; set; }
    public string FormulaStoryJson { get; set; }
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
}