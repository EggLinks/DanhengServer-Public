using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RogueDLCArea.json")]
    public class RogueDLCAreaExcel : ExcelResource
    {
        public int AreaID { get; set; }
        public string SubType { get; set; } = "";
        public List<int> LayerIDList { get; set; } = [];
        public List<int> DifficultyID { get; set; } = [];
        public int FirstReward { get; set; }

        public List<RogueDLCAreaScoreMap> AreaScoreMap { get; set; } = [];

        [JsonIgnore]
        public int RogueVersionId { get; set; }

        public override int GetId()
        {
            return AreaID;
        }

        public override void Loaded()
        {
            GameData.RogueDLCAreaData[AreaID] = this;

            if (SubType.Contains("Nous"))
            {
                RogueVersionId = 202;
            }
            else
            {
                 RogueVersionId = 201;
            }
        }
    }

    public class RogueDLCAreaScoreMap
    {
        [JsonProperty("NALLPFKBHIO")]
        public int Layer { get; set; }

        [JsonProperty("GGOHPEDAKJE")]
        public int ExploreScore { get; set; }

        [JsonProperty("BELDLJADLKO")]
        public int FinishScore { get; set; }
    }
}
