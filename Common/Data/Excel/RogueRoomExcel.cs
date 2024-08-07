namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueRoom.json")]
public class RogueRoomExcel : ExcelResource
{
    public int RogueRoomID { get; set; }
    public int RogueRoomType { get; set; }
    public int MapEntrance { get; set; }
    public int GroupID { get; set; }
    public Dictionary<int, int> GroupWithContent { get; set; } = [];

    public override int GetId()
    {
        return RogueRoomID;
    }

    public override void Loaded()
    {
        GameData.RogueRoomData.Add(RogueRoomID, this);
    }
}