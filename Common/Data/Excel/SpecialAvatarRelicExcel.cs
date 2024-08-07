using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("SpecialAvatarRelic.json")]
public class SpecialAvatarRelicExcel : ExcelResource
{
    public int RelicPropertyType { get; set; }
    public List<SpecialAvatarRelicInfo> RelicIDList { get; set; } = [];

    public override int GetId()
    {
        return RelicPropertyType;
    }

    public override void Loaded()
    {
        GameData.SpecialAvatarRelicData[GetId()] = this;
    }
}

public class SpecialAvatarRelicInfo
{
    [JsonProperty("BDHIKPAMCJF")] public int RelicID { get; set; }

    [JsonProperty("PKAFFFBFJII")] public int RelicLevel { get; set; }
}