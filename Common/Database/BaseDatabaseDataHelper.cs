using SqlSugar;

namespace EggLink.DanhengServer.Database;

public abstract class BaseDatabaseDataHelper
{
    [SugarColumn(IsPrimaryKey = true)] public int Uid { get; set; }
}