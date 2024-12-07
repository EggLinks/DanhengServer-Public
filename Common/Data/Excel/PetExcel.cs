namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("PetConfig.json")]
public class PetExcel : ExcelResource
{
    public int PetID { get; set; }
    public int PetItemID { get; set; }
    public int SummonUnitID { get; set; }

    public override int GetId()
    {
        return PetID;
    }

    public override void Loaded()
    {
        GameData.PetData.Add(PetID, this);
    }
}