using EggLink.DanhengServer.Enums.TournRogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournWorkbenchFunc.json")]
public class RogueTournWorkbenchFuncExcel : ExcelResource
{
    public int FuncID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueTournWorkbenchFuncTypeEnum FuncType { get; set; }

    public override int GetId()
    {
        return FuncID;
    }

    public override void Loaded()
    {
        GameData.RogueTournWorkbenchFuncData.Add(FuncID, this);
    }
}