using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.RogueMagic.MagicUnit;

public class RogueMagicUnitInstance(RogueMagicUnitExcel excel)
{
    public RogueMagicUnitExcel Excel { get; set; } = excel;
    public int UniqueId { get; set; }

    public RogueMagicGameUnitInfo ToProto()
    {
        return new RogueMagicGameUnitInfo
        {
            UniqueId = (uint)UniqueId,
            GameMagicUnit = ToBasicInfo()
        };
    }

    public RogueMagicGameUnit ToBasicInfo()
    {
        return new RogueMagicGameUnit
        {
            MagicUnitId = (uint)Excel.MagicUnitID,
            Level = (uint)Excel.MagicUnitLevel
        };
    }

    public RogueCommonActionResult ToGetInfo(RogueCommonActionResultSourceType source)
    {
        return new RogueCommonActionResult
        {
            Source = source,
            RogueAction = new RogueCommonActionResultData
            {
                GetMagicUnitList = ToProto()
            }
        };
    }

    public RogueCommonActionResult ToRemoveInfo(RogueCommonActionResultSourceType source)
    {
        return new RogueCommonActionResult
        {
            Source = source,
            RogueAction = new RogueCommonActionResultData
            {
                RemoveMagicUnitList = ToProto()
            }
        };
    }
}