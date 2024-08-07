using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("DialogueEvent.json")]
public class DialogueEventExcel : ExcelResource
{
    public int EventID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public DialogueEventTypeEnum RogueEffectType { get; set; }

    public List<int> RogueEffectParamList { get; set; } = [];

    [JsonConverter(typeof(StringEnumConverter))]
    public DialogueEventCostTypeEnum CostType { get; set; }

    public List<int> CostParamList { get; set; } = [];

    public int DynamicContentID { get; set; }
    public int DescValue { get; set; }

    public override int GetId()
    {
        return EventID;
    }

    public override void Loaded()
    {
        GameData.DialogueEventData.Add(EventID, this);
    }
}