using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.EffectHandler
{
    [RogueEvent(DialogueEventTypeEnum.GetAllRogueBuffInGroup)]
    public class EventHandlerGetAllRogueBuffInGroup : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            var group = ParamList[0];
            GameData.RogueBuffGroupData.TryGetValue(group, out var buffGroup);
            if (buffGroup == null) return;
            rogue.AddBuffList(buffGroup.BuffList);
        }
    }
}
