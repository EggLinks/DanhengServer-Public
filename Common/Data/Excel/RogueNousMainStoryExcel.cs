namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueNousMainStory.json")]
public class RogueNousMainStoryExcel : ExcelResource
{
    public int StoryID { get; set; }
    public int Layer { get; set; }
    public int RogueNPCID { get; set; }
    public int QuestID { get; set; }
    public int StoryGroup { get; set; }

    public override int GetId()
    {
        return StoryID;
    }

    public override void Loaded()
    {
        GameData.RogueNousMainStoryData.Add(GetId(), this);
    }
}