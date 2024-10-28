namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournHandbookMiracle.json")]
public class RogueTournHandbookMiracleExcel : ExcelResource
{
    public int HandbookMiracleID { get; set; }
    public int MiracleDisplayID { get; set; }

    public override int GetId()
    {
        return HandbookMiracleID;
    }

    public override void Loaded()
    {
        GameData.RogueTournHandbookMiracleData.TryAdd(GetId(), this);
    }
}