using EggLink.DanhengServer.Enums.Scene;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("MazePlane.json", true)]
    public class MazePlaneExcel : ExcelResource
    {
        public int PlaneID { get; set; }
        public int WorldID { get; set; }
        public int StartFloorID { get; set; }
        public HashName PlaneName { get; set; } = new();

        [JsonConverter(typeof(StringEnumConverter))]
        public PlaneTypeEnum PlaneType { get; set; }

        public override int GetId()
        {
            return PlaneID;
        }

        public override void Loaded()
        {
            GameData.MazePlaneData.Add(PlaneID, this);
        }
    }
}
