using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("MessageItemConfig.json")]
    public class MessageItemConfigExcel : ExcelResource
    {
        public int ID { get; set; }
        public List<int> NextItemIDList { get; set; } = [];
        public int SectionID { get; set; }

        [JsonIgnore]
        public int GroupID { get; set; }

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.MessageItemConfigData.Add(ID, this);
        }

        public override void AfterAllDone()
        {
            if (GameData.MessageSectionConfigData.TryGetValue(SectionID, out MessageSectionConfigExcel? value))
            {
                value.Items.Add(this);
                GroupID = value.GroupID;
            }
        }
    }
}
