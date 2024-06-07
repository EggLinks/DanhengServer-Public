using EggLink.DanhengServer.Enums;

namespace EggLink.DanhengServer.Game.Mission.FinishType
{
    public class MissionFinishTypeAttribute(MissionFinishTypeEnum finishType) : Attribute
    {
        public MissionFinishTypeEnum FinishType { get; private set; } = finishType;
    }
}
