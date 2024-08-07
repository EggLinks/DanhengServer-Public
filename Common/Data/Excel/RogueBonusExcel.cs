namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueBonus.json")]
public class RogueBonusExcel : ExcelResource
{
    public int BonusID { get; set; }
    public int BonusEvent { get; set; }

    public override int GetId()
    {
        return BonusID;
    }

    public override void Loaded()
    {
        GameData.RogueBonusData.Add(GetId(), this);
    }
}