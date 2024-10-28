namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournHexAvatarBaseType.json")]
public class RogueTournHexAvatarBaseTypeExcel : ExcelResource
{
    public int MiracleID { get; set; }

    public override int GetId()
    {
        return MiracleID;
    }

    public override void Loaded()
    {
        GameData.RogueTournHexAvatarBaseTypeData.TryAdd(MiracleID, this);
    }
}