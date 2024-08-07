using EggLink.DanhengServer.Enums.Mission;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction;

[AttributeUsage(AttributeTargets.Class)]
public class MissionFinishActionAttribute(FinishActionTypeEnum finishAction) : Attribute
{
    public FinishActionTypeEnum FinishAction { get; } = finishAction;
}