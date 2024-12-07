namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMagicTalent.json")]
public class RogueMagicTalentExcel : ExcelResource
{
    public int TalentID { get; set; }
    public int Level { get; set; }

    public override int GetId()
    {
        return TalentID;
    }

    public override void Loaded()
    {
        GameData.RogueMagicTalentData.Add(TalentID, this);
    }
}