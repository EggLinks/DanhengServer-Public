using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("EquipmentPromotionConfig.json")]
    public class EquipmentPromotionConfigExcel : ExcelResource
    {
        public int EquipmentID { get; set; }
        public int Promotion { get; set; }
        public int MaxLevel { get; set; }

        public override int GetId()
        {
            return EquipmentID * 10 + Promotion;
        }

        public override void Loaded()
        {
            GameData.EquipmentPromotionConfigData.Add(GetId(), this);
        }
    }
}
