using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RogueMazeBuff.json")]
    public class RogueMazeBuffExcel : ExcelResource
    {
        public int ID { get; set; }
        public int Lv { get; set; }
        public int LvMax { get; set; }
        public HashName BuffName { get; set; } = new();

        [JsonIgnore]
        public string? Name;

        public override int GetId()
        {
            return ID * 100 + Lv;
        }

        public override void Loaded()
        {
            GameData.RogueMazeBuffData.Add(GetId(), this);
        }
    }
}
