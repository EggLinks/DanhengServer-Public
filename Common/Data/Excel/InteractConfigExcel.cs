using System.Text.Json.Serialization;
using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("InteractConfig.json")]
public class InteractConfigExcel : ExcelResource
{
    public int InteractID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PropStateEnum SrcState { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PropStateEnum TargetState { get; set; } = PropStateEnum.Closed;

    public override int GetId()
    {
        return InteractID;
    }

    public override void Loaded()
    {
        GameData.InteractConfigData.Add(InteractID, this);
    }
}