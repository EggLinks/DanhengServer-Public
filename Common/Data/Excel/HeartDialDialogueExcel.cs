namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("HeartDialDialogue.json")]
public class HeartDialDialogueExcel : ExcelResource
{
    public int ID { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.HeartDialDialogueData[ID] = this;
    }
}