using EggLink.DanhengServer.Data.Config.Rogue;

namespace EggLink.DanhengServer.Data.Excel;

[ResourceEntity("RogueNPC.json,RogueTournNPC.json", true)]
public class RogueNPCExcel : ExcelResource
{
    public int RogueNPCID { get; set; }
    public string NPCJsonPath { get; set; } = string.Empty;

    public RogueNPCConfigInfo? RogueNpcConfig { get; set; }

    public override int GetId()
    {
        return RogueNPCID;
    }

    public override void Loaded()
    {
        GameData.RogueNPCData.TryAdd(RogueNPCID, this);
    }

    public bool CanUseInVer(int version)
    {
        return RogueNpcConfig != null;
    }
}