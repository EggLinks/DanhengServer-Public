namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MusicRhythmGroup.json")]
public class MusicRhythmGroupExcel : ExcelResource
{
    public int ID { get; set; }
    public int Index { get; set; }
    public int Phase { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MusicRhythmGroupData.Add(ID, this);
    }
}