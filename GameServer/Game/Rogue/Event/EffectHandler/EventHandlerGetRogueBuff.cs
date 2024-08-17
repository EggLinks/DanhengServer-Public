using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.EffectHandler;

[RogueEvent(DialogueEventTypeEnum.GetRogueBuff)]
public class EventHandlerGetRogueBuff : RogueEventEffectHandler
{
    public override void Init(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> paramList,
        RogueEventParam? option)
    {
    }

    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList, RogueEventParam? option)
    {
        var groupId = paramList[0];
        GameData.RogueBuffGroupData.TryGetValue(groupId, out var buffGroup);
        if (buffGroup == null) return;
        for (var i = 0; i < paramList[1]; i++)
        {
            var buff = buffGroup.BuffList.RandomElement();
            await rogue.AddBuff(buff.MazeBuffID, buff.MazeBuffLevel);
        }
    }
}