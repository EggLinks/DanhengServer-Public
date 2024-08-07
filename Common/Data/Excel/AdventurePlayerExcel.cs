namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("AdventurePlayer.json")]
public class AdventurePlayerExcel : ExcelResource
{
    public int AvatarID { get; set; } = 0;
    public string PlayerJsonPath { get; set; } = "";

    public override int GetId()
    {
        return AvatarID;
    }

    public override void Loaded()
    {
        GameData.AdventurePlayerData[AvatarID] = this;
    }
}