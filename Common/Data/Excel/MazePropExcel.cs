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
    [ResourceEntity("MazeProp.json")]
    public class MazePropExcel : ExcelResource
    {
        public int ID { get; set; }
        public HashName PropName { get; set; } = new();
        public string JsonPath { get; set; } = "";

        [JsonConverter(typeof(StringEnumConverter))]
        public PropTypeEnum PropType { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<PropStateEnum> PropStateList { get; set; } = [];

        public bool IsHpRecover = false;
        public bool IsMpRecover = false;
        public bool IsDoor = false;

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            if (JsonPath != "")
            {
                if (JsonPath.Contains("MPBox") || JsonPath.Contains("MPRecover"))
                {
                    IsMpRecover = true;
                } else if (JsonPath.Contains("HPBox") || JsonPath.Contains("HPRecover"))
                {
                    IsHpRecover = true;
                } else if (JsonPath.Contains("_Door_"))
                {
                    IsDoor = true;
                }
            }

            GameData.MazePropData.Add(ID, this);
        }
    }
}
