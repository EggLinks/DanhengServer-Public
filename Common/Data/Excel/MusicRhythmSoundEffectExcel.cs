namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MusicRhythmSoundEffect.json")]
public class MusicRhythmSoundEffectExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.MusicRhythmSoundEffectData.Add(ID, this);
    }
}