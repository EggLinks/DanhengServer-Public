using EggLink.DanhengServer.Proto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Database.Tutorial
{
    [SugarTable("TutorialGuide")]
    public class TutorialGuideData : BaseDatabaseDataHelper
    {
        [SugarColumn(IsJson = true)]
        public Dictionary<int, TutorialStatus> Tutorials { get; set; } = [];
    }
}
