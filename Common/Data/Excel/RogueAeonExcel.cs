namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueAeon.json")]
public class RogueAeonExcel : ExcelResource
{
    public int AeonID { get; set; }
    public int RogueVersion { get; set; }

    public int RogueBuffType { get; set; }
    public int BattleEventBuffGroup { get; set; }
    public int BattleEventEnhanceBuffGroup { get; set; }

    public override int GetId()
    {
        return AeonID;
    }

    public override void Loaded()
    {
        GameData.RogueAeonData.Add(GetId(), this);
    }
}