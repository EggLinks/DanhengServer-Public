namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMagicStory.json")]
public class RogueMagicStoryExcel : ExcelResource
{
    public int StoryID { get; set; }
    public bool IsHide { get; set; }
    public int UnLockDisplay { get; set; }

    public override int GetId()
    {
        return StoryID;
    }

    public override void Loaded()
    {
        GameData.RogueMagicStoryData.Add(StoryID, this);
    }
}