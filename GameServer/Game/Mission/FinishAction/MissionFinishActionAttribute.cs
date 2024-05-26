using EggLink.DanhengServer.Enums;

namespace EggLink.DanhengServer.Game.Mission.FinishAction
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MissionFinishActionAttribute(FinishActionTypeEnum finishAction) : Attribute
    {
        public FinishActionTypeEnum FinishAction { get; } = finishAction;
    }
}
