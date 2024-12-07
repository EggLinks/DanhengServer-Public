using EggLink.DanhengServer.Data.Custom;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueTournBuff.json")]
public class RogueTournBuffExcel : BaseRogueBuffExcel
{
    public bool IsInHandbook { get; set; }

    public override int GetId()
    {
        return MazeBuffID * 100 + MazeBuffLevel;
    }

    public override void Loaded()
    {
        GameData.RogueBuffData.TryAdd(GetId(), this);
    }
}