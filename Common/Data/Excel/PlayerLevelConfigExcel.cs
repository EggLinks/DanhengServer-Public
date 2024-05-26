using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("PlayerLevelConfig.json")]
    public class PlayerLevelConfigExcel : ExcelResource
    {
        public int Level { get; set; }
        public int PlayerExp { get; set; }
        public int StaminaLimit { get; set; }
        public int LevelRewardID { get; set; }

        public override int GetId()
        {
            return Level;
        }

        public override void Loaded()
        {
            GameData.PlayerLevelConfigData.Add(Level, this);
        }
    }
}
