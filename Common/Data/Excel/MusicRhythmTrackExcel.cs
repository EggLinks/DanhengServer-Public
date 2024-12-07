namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MusicRhythmTrack.json")]
public class MusicRhythmTrackExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MusicRhythmTrackData.Add(ID, this);
    }
}