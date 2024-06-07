using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("BackGroundMusic.json")]
    public class BackGroundMusicExcel : ExcelResource
    {
        public int ID { get; set; }
        public int GroupID { get; set; }

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.BackGroundMusicData[ID] = this;
        }
    }
}
