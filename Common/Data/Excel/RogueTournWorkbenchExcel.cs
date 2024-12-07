using Newtonsoft.Json;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournWorkbench.json")]
public class RogueTournWorkbenchExcel : ExcelResource
{
    public int WorkbenchID { get; set; }
    public List<int> FuncList { get; set; } = [];

    [JsonIgnore] public List<RogueTournWorkbenchFuncExcel> Funcs { get; set; } = [];

    public override int GetId()
    {
        return WorkbenchID;
    }

    public override void Loaded()
    {
        GameData.RogueTournWorkbenchData.Add(WorkbenchID, this);
    }

    public override void AfterAllDone()
    {
        foreach (var func in FuncList)
            if (GameData.RogueTournWorkbenchFuncData.TryGetValue(func, out var funcExcel))
                Funcs.Add(funcExcel);
    }
}