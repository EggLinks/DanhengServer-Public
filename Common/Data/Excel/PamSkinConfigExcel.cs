namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("PamSkinConfig.json")]
public class PamSkinConfigExcel : ExcelResource
{
    public int SkinID { get; set; }

    public override int GetId()
    {
        return SkinID;
    }

    public override void Loaded()
    {
        GameData.PamSkinConfigData.Add(SkinID, this);
    }
}