namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("TrainPartyTeam.json")]
public class TrainPartyTeamExcel : ExcelResource
{
    public int TeamID { get; set; }
    public int InitialMeetingSkill { get; set; }
    public int LeaderWorkingBuffID { get; set; }
    public int GridNum { get; set; }
    public List<int> PassengerList { get; set; } = [];

    public override int GetId()
    {
        return TeamID;
    }

    public override void Loaded()
    {
        GameData.TrainPartyTeamData.Add(TeamID, this);
    }
}