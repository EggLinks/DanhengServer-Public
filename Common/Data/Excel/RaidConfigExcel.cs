using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RaidConfig.json")]
public class RaidConfigExcel : ExcelResource
{
    public int RaidID { get; set; }
    public int HardLevel { get; set; }

    public int FinishEntranceID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RaidTeamTypeEnum TeamType { get; set; }

    public List<int> TrialAvatarList { get; set; } = [];
    public List<int> MainMissionIDList { get; set; } = [];

    public bool LockCaptain { get; set; }
    public int LockCaptainAvatarID { get; set; }

    public override int GetId()
    {
        return RaidID * 100 + HardLevel;
    }

    public override void Loaded()
    {
        GameData.RaidConfigData.Add(GetId(), this);
    }
}