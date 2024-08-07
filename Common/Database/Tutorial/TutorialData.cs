using EggLink.DanhengServer.Proto;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Tutorial;

[SugarTable("Tutorial")]
public class TutorialData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public Dictionary<int, TutorialStatus> Tutorials { get; set; } = [];
}