using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueBuff.json")]
public class RogueBuffExcel : BaseRogueBuffExcel
{
    public int AeonID { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RogueBuffAeonTypeEnum BattleEventBuffType { get; set; } = RogueBuffAeonTypeEnum.Normal;

    public bool IsAeonBuff => BattleEventBuffType != RogueBuffAeonTypeEnum.Normal;

    public override int GetId()
    {
        return MazeBuffID * 100 + MazeBuffLevel;
    }

    public override void Loaded()
    {
        GameData.RogueBuffData.Add(GetId(), this);

        if (BattleEventBuffType == RogueBuffAeonTypeEnum.BattleEventBuff)
        {
            GameData.RogueAeonBuffData.Add(AeonID, this);
        }
        else if (BattleEventBuffType == RogueBuffAeonTypeEnum.BattleEventBuffEnhance)
        {
            if (GameData.RogueAeonEnhanceData.TryGetValue(AeonID, out var aeonBuff))
                aeonBuff.Add(this);
            else
                GameData.RogueAeonEnhanceData.Add(AeonID, [this]);
        }
    }
}