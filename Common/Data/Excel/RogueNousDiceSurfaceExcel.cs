namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueNousDiceSurface.json")]
public class RogueNousDiceSurfaceExcel : ExcelResource
{
    public int SurfaceID { get; set; }
    public int ItemID { get; set; }
    public int Sort { get; set; }
    public int DiceActiveStage { get; set; }
    public HashName SurfaceName { get; set; } = new();
    public HashName SurfaceDesc { get; set; } = new();

    public override int GetId()
    {
        return SurfaceID;
    }

    public override void Loaded()
    {
        GameData.RogueNousDiceSurfaceData[SurfaceID] = this;
    }
}