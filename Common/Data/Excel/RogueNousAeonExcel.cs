namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueNousAeon.json")]
public class RogueNousAeonExcel : ExcelResource
{
    public int AeonID { get; set; }
    public int RogueBuffType { get; set; }
    public List<int> EffectParam1 { get; set; } = [];

    public int BattleEventBuffGroup { get; set; }
    public int BattleEventEnhanceBuffGroup { get; set; }

    public override int GetId()
    {
        return AeonID;
    }

    public override void Loaded()
    {
        GameData.RogueNousAeonData[AeonID] = this;
    }
}