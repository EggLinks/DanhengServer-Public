using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.GetRogueBuff)]
    public class EventHandlerGetRogueBuff : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            var groupId = ParamList[0];
            GameData.RogueBuffGroupData.TryGetValue(groupId, out var buffGroup);
            if (buffGroup == null) return;
            for (int i = 0; i < ParamList[1]; i++)
            {
                var buff = buffGroup.BuffList.RandomElement();
                rogue.AddBuff(buff.MazeBuffID, buff.MazeBuffLevel);
            }
        }
    }
}
