namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueNousTalent.json")]
public class RogueNousTalentExcel : ExcelResource
{
    public int TalentID { get; set; }

    public override int GetId()
    {
        return TalentID;
    }

    public override void Loaded()
    {
        GameData.RogueNousTalentData[TalentID] = this;
    }
}