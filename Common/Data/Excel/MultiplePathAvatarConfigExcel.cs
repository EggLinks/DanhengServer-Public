using EggLink.DanhengServer.Enums.Quest;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("MultiplePathAvatarConfig.json")]
public class MultiplePathAvatarConfigExcel : ExcelResource
{
    public List<Condition>? UnlockConditions = new();
    public string ChangeConfigPath { get; set; } = "";
    public string Gender { get; set; } = "";
    public int AvatarID { get; set; }
    public int BaseAvatarID { get; set; }

    public override int GetId()
    {
        return AvatarID;
    }

    public override void Loaded()
    {
        GameData.MultiplePathAvatarConfigData.Add(AvatarID, this);
    }
}

public class Condition
{
    public string Param { get; set; } = "";
    public ConditionTypeEnum Type { get; set; }
}