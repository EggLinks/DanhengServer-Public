using EggLink.DanhengServer.Enums.Rogue;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("DialogueDynamicContent.json")]
    public class DialogueDynamicContentExcel : ExcelResource
    {
        public int DynamicContentID { get; set; }
        public int ArgID { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DialogueDynamicParamTypeEnum DynamicParamType { get; set; }
        public List<int> DynamicParamList { get; set; } = [];

        public override int GetId()
        {
            return DynamicContentID;
        }

        public override void Loaded()
        {
            if (GameData.DialogueDynamicContentData.TryGetValue(DynamicContentID, out Dictionary<int, DialogueDynamicContentExcel>? value))
            {
                value.Add(ArgID, this);
            }
            else
            {
                GameData.DialogueDynamicContentData.Add(DynamicContentID, new() { { ArgID, this } });
            }
        }
    }
}
