using EggLink.DanhengServer.Enums.Mission;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishType;

public class MissionFinishTypeAttribute(MissionFinishTypeEnum finishType) : Attribute
{
    public MissionFinishTypeEnum FinishType { get; private set; } = finishType;
}