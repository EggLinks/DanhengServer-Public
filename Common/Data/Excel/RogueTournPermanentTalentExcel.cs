namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournPermanentTalent.json")]
public class RogueTournPermanentTalentExcel : ExcelResource
{
    public int TalentID { get; set; }
    public List<int> NextTalentIDList { get; set; } = [];

    public override int GetId()
    {
        return TalentID;
    }

    public override void Loaded()
    {
        GameData.RogueTournPermanentTalentData.TryAdd(TalentID, this);
    }
}