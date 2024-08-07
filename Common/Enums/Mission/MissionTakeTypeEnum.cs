namespace EggLink.DanhengServer.Enums.Mission;

public enum MissionTakeTypeEnum
{
    MultiSequence = 0,
    Auto = 1,
    PlayerLevel = 2,
    WorldLevel = 3,
    Manual = 4,
    SequenceNextDay = 5,
    MuseumPhaseRenewPointReach = 6,
    HeliobusPhaseReach = 7
}

public enum SubMissionTakeTypeEnum
{
    Unknown = 0,
    AnySequence = 2,
    Auto = 1,
    CustomValue = 4,
    MultiSequence = 3
}