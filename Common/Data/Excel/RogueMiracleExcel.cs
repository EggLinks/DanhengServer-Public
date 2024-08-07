using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMiracle.json")]
public class RogueMiracleExcel : ExcelResource
{
    public int MiracleID { get; set; }
    public int MiracleDisplayID { get; set; }
    public int UnlockHandbookMiracleID { get; set; }

    [JsonIgnore] public HashName MiracleName { get; set; } = new();

    [JsonIgnore] public string? Name { get; set; }

    public override int GetId()
    {
        return MiracleID;
    }

    public override void AfterAllDone()
    {
        MiracleName = GameData.RogueMiracleDisplayData[MiracleDisplayID].MiracleName;
        GameData.RogueMiracleData[MiracleID] = this;
    }
}