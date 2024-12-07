namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MusicRhythmSong.json")]
public class MusicRhythmSongExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MusicRhythmSongData.Add(ID, this);
    }
}