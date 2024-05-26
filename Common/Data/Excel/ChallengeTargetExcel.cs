using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("ChallengeTargetConfig.json,ChallengeStoryTargetConfig.json", isMultifile: true)]
    public class ChallengeTargetExcel : ExcelResource
    {
        public int ID { get; set; }
        public ChallengeType ChallengeTargetType { get; set; }
        public int ChallengeTargetParam1 { get; set; }

        public override int GetId()
        {
            return ID;
        }

        public override void Loaded()
        {
            GameData.ChallengeTargetData[ID] = this;
        }

        public enum ChallengeType
        {
            None, ROUNDS, DEAD_AVATAR, KILL_MONSTER, AVATAR_BASE_TYPE_MORE, AVATAR_BASE_TYPE_LESS, ROUNDS_LEFT, TOTAL_SCORE
        }
    }
}
