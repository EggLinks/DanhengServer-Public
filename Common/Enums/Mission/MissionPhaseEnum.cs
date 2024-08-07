using EggLink.DanhengServer.Enums.Task;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Enums.Mission;

public enum MissionPhaseEnum
{
    Accept = 0,
    Finish = 1,
    None = 3,
    Cancel = 4
}

public static class MissionStatusExtensions
{
    public static MissionStatus ToProto(this MissionPhaseEnum status)
    {
        return status switch
        {
            MissionPhaseEnum.None => MissionStatus.MissionNone,
            MissionPhaseEnum.Accept => MissionStatus.MissionDoing,
            MissionPhaseEnum.Finish => MissionStatus.MissionFinish,
            MissionPhaseEnum.Cancel => MissionStatus.MissionNone,
            _ => MissionStatus.MissionNone
        };
    }

    public static SubMissionStateEnum ToStateEnum(this MissionPhaseEnum status)
    {
        return status switch
        {
            MissionPhaseEnum.None => SubMissionStateEnum.Unknow,
            MissionPhaseEnum.Accept => SubMissionStateEnum.Started,
            MissionPhaseEnum.Finish => SubMissionStateEnum.Finish,
            MissionPhaseEnum.Cancel => SubMissionStateEnum.TakenAndNotStarted,
            _ => SubMissionStateEnum.Unknow
        };
    }
}