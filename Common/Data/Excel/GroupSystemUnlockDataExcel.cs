namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("GroupSystemUnlockData.json")]
public class GroupSystemUnlockDataExcel : ExcelResource
{
    public int GroupSystemUnlockID { get; set; }
    public int UnlockID { get; set; }

    public override int GetId()
    {
        return GroupSystemUnlockID;
    }

    public override void Loaded()
    {
        GameData.GroupSystemUnlockDataData[GroupSystemUnlockID] = this;
    }
}