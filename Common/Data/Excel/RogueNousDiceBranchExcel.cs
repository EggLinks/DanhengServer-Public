namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueNousDiceBranch.json")]
public class RogueNousDiceBranchExcel : ExcelResource
{
    public int BranchID { get; set; }
    public int DefaultUltraSurface { get; set; }
    public List<int> DefaultCommonSurfaceList { get; set; } = [];

    public override int GetId()
    {
        return BranchID;
    }

    public override void Loaded()
    {
        GameData.RogueNousDiceBranchData[BranchID] = this;
    }

    public List<int> GetDefaultSurfaceList()
    {
        var list = new List<int>
        {
            DefaultUltraSurface
        };
        list.AddRange(DefaultCommonSurfaceList);

        return list;
    }
}