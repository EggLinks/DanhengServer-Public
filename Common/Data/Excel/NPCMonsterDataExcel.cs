using EggLink.DanhengServer.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("NPCMonsterData.json")]
    public class NPCMonsterDataExcel : ExcelResource
    { 
        public int ID { get; set; }
        public HashName NPCName { get; set; } = new();

        [JsonConverter(typeof(StringEnumConverter))]
        public MonsterRankEnum Rank { get; set; }

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.NpcMonsterDataData.Add(ID, this);
        }
    }
}
