namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMiracleDisplay.json")]
public class RogueMiracleDisplayExcel : ExcelResource
{
    public int MiracleDisplayID { get; set; }
    public HashName MiracleName { get; set; } = new();

    public override int GetId()
    {
        return MiracleDisplayID;
    }

    public override void Loaded()
    {
        GameData.RogueMiracleDisplayData.Add(GetId(), this);
    }
}