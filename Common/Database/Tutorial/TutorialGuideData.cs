using EggLink.DanhengServer.Proto;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Tutorial;

[SugarTable("TutorialGuide")]
public class TutorialGuideData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public Dictionary<int, TutorialStatus> Tutorials { get; set; } = [];
}