using EggLink.DanhengServer.Database.Lineup;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using SqlSugar;

namespace EggLink.DanhengServer.Database.Scene;

[SugarTable("RaidData")]
public class RaidData : BaseDatabaseDataHelper
{
    [SugarColumn(IsJson = true)] public Dictionary<int, Dictionary<int, RaidRecord>> RaidRecordDatas { get; set; } = [];

    [SugarColumn(IsJson = true)]
    [Obsolete("Using RaidRecordDatas")]
    public Dictionary<int, RaidRecord> RaidRecordData { get; set; } = [];

    //[SugarColumn(IsJson = true, IsNullable = true)]
    //public Position? OldPos { get; set; }

    //[SugarColumn(IsJson = true, IsNullable = true)]
    //public Position? OldRot { get; set; }

    //public int OldEntryId { get; set; }
    public int CurRaidId { get; set; }
    public int CurRaidWorldLevel { get; set; }
}

public class RaidRecord
{
    // Basic Info
    public int RaidId { get; set; }
    public int WorldLevel { get; set; }
    public RaidStatus Status { get; set; }
    public long FinishTimeStamp { get; set; }

    // Lineup Info
    public List<LineupAvatarInfo> Lineup { get; set; } = [];

    // Scene Info
    public Position Pos { get; set; } = new();
    public Position Rot { get; set; } = new();
    public int PlaneId { get; set; }
    public int FloorId { get; set; }
    public int EntryId { get; set; }

    public Position OldPos { get; set; } = new();
    public Position OldRot { get; set; } = new();
    public int OldEntryId { get; set; }
}