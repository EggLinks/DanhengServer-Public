using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("AvatarPromotionConfig.json")]
    public class AvatarPromotionConfigExcel : ExcelResource
    {
        public int AvatarID { get; set; }
        public int Promotion { get; set; }
        public int MaxLevel { get; set; }

        public override int GetId()
        {
            return AvatarID * 10 + Promotion;
        }

        public override void Loaded()
        {
            GameData.AvatarPromotionConfigData.Add(GetId(), this);
        }
    }
}
