using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("RelicExpItem.json")]
    public class RelicExpItemExcel : ExcelResource
    {
        public int ItemID { get; set; }
        public int ExpProvide { get; set; }
        public int CoinCost { get; set; }

        public override int GetId()
        {
            return ItemID;
        }

        public override void Loaded()
        {
            GameData.RelicExpItemData[ItemID] = this;
        }
    }
}
