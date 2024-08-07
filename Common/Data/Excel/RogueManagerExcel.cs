using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueManager.json")]
public class RogueManagerExcel : ExcelResource
{
    public int RogueSeason { get; set; }
    public string BeginTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public List<int> RogueAreaIDList { get; set; } = [];

    [JsonIgnore] public DateTime BeginTimeDate { get; set; }

    [JsonIgnore] public DateTime EndTimeDate { get; set; }

    public override int GetId()
    {
        return RogueSeason;
    }

    public override void Loaded()
    {
        GameData.RogueManagerData.Add(GetId(), this);
        BeginTimeDate = DateTime.Parse(BeginTime);
        EndTimeDate = DateTime.Parse(EndTime);
    }
}