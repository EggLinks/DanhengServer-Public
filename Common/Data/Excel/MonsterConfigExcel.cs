namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MonsterConfig.json")]
public class MonsterConfigExcel : ExcelResource
{
    public int MonsterID { get; set; }

    public override int GetId()
    {
        return MonsterID;
    }

    public override void Loaded()
    {
        GameData.MonsterConfigData.Add(MonsterID, this);
    }
}