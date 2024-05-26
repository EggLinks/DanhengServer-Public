using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Proto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RogueBuff.json")]
    public class RogueBuffExcel : ExcelResource
    {
        public int MazeBuffID { get; set; }
        public int MazeBuffLevel { get; set; }
        public int RogueBuffType { get; set; }
        public int RogueBuffRarity { get; set; }
        public int RogueBuffTag { get; set; }
        public int AeonID { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RogueBuffAeonTypeEnum BattleEventBuffType { get; set; } = RogueBuffAeonTypeEnum.Normal;

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
            } else if (BattleEventBuffType == RogueBuffAeonTypeEnum.BattleEventBuffEnhance)
            {
                if (GameData.RogueAeonEnhanceData.TryGetValue(AeonID, out var aeonBuff))
                {
                    aeonBuff.Add(this);
                }
                else
                {
                    GameData.RogueAeonEnhanceData.Add(AeonID, [this]);
                }
            }
        }

        public RogueCommonBuff ToProto()
        {
            return new()
            {
                BuffId = (uint)MazeBuffID,
                BuffLevel = (uint)MazeBuffLevel,
            };
        }

        public bool IsAeonBuff => BattleEventBuffType != RogueBuffAeonTypeEnum.Normal;
    }
}
