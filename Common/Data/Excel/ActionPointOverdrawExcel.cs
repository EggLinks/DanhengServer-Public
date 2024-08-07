namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("ActionPointOverdraw.json")]
public class ActionPointOverdrawExcel : ExcelResource
{
    public int ActionPoint { get; set; }
    public int MazeBuff { get; set; }

    public override int GetId()
    {
        return ActionPoint;
    }

    public override void Loaded()
    {
        GameData.ActionPointOverdrawData.Add(ActionPoint, this);
    }
}