namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueMonsterGroup.json")]
public class RogueMonsterGroupExcel : ExcelResource
{
    public Dictionary<string, int> RogueMonsterListAndWeight { get; set; } = [];
    public int RogueMonsterGroupID { get; set; }
    public int EliteGroup { get; set; }

    public override int GetId()
    {
        return RogueMonsterGroupID;
    }

    public override void Loaded()
    {
        GameData.RogueMonsterGroupData.Add(GetId(), this);
    }
}