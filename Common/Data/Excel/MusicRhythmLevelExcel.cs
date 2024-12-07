namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MusicRhythmLevel.json")]
public class MusicRhythmLevelExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MusicRhythmLevelData.Add(ID, this);
    }
}