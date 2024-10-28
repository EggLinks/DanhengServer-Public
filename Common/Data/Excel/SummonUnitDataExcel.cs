using EggLink.DanhengServer.Data.Config.SummonUnit;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("SummonUnitData.json")]
public class SummonUnitDataExcel : ExcelResource
{
    public int ID { get; set; }
    public string JsonPath { get; set; } = "";
    public bool DestroyOnEnterBattle { get; set; }
    public bool RemoveMazeBuffOnDestroy { get; set; }

    public bool IsClient { get; set; }

    public SummonUnitConfigInfo? ConfigInfo { get; set; }

    public override int GetId()
    {
        return ID;
    }

    public override void Loaded()
    {
        GameData.SummonUnitDataData[ID] = this;
    }
}