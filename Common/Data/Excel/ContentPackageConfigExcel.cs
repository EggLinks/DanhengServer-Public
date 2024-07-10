using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("ContentPackageConfig.json")]
    public class ContentPackageConfigExcel : ExcelResource
    {
        public int ContentID { get; set; }

        public override int GetId()
        {
            return ContentID;
        }

        public override void Loaded()
        {
            GameData.ContentPackageConfigData.Add(ContentID, this);
        }
    }
}
