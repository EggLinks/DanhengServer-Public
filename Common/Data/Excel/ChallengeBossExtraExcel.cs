using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("ChallengeBossMazeExtra.json")]
    public class ChallengeBossExtraExcel : ExcelResource
    {
        public int ID { get; set; }
        public int MonsterID1 { get; set; }
        public int MonsterID2 { get; set; }


        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            if (GameData.ChallengeConfigData.ContainsKey(ID))
            {
                var challengeExcel = GameData.ChallengeConfigData[ID];
                challengeExcel.SetBossExcel(this);
            }
        }
    }
}
