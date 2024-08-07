using EggLink.DanhengServer.Data.Config;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueDLCChessBoard.json")]
public class RogueDLCChessBoardExcel : ExcelResource
{
    public int ChessBoardID { get; set; }
    public string ChessBoardConfiguration { get; set; } = string.Empty;

    [JsonIgnore] public RogueChestMapInfo? MapInfo { get; set; }

    public override int GetId()
    {
        return ChessBoardID;
    }

    public override void Loaded()
    {
        if (ChessBoardID.ToString().StartsWith("201"))
        {
            var layer = int.Parse(ChessBoardID.ToString().Substring(3, 1));
            if (!GameData.RogueSwarmChessBoardData.TryGetValue(layer, out var value))
            {
                value = [];
                GameData.RogueSwarmChessBoardData[layer] = value;
            }

            value.Add(this);
        }
        else if (ChessBoardID.ToString().StartsWith("211"))
        {
            var layer = int.Parse(ChessBoardID.ToString().Substring(3, 1));
            if (!GameData.RogueNousChessBoardData.TryGetValue(layer, out var value))
            {
                value = [];
                GameData.RogueNousChessBoardData[layer] = value;
            }

            value.Add(this);
        }
    }
}