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
    [RogueEvent(DialogueEventTypeEnum.GetRogueMiracle)]
    public class EventHandlerGetRogueMiracle : RogueEventEffectHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            var miracleGroupId = ParamList[0];
            GameData.RogueMiracleGroupData.TryGetValue(miracleGroupId, out var miracleGroup);
            if (miracleGroup == null) return;

            var list = new List<int>();
            foreach (var id in miracleGroup)
            {
                if (!rogue.RogueMiracles.ContainsKey(id))
                {
                    // Add the miracle to the list if the player doesn't have it
                    list.Add(id);
                }
            }

            if (list.Count == 0) return;  // If the player already has all the miracles in the group, return

            for (int i = 0; i < ParamList[1]; i++)
            {
                if (list.Count == 0) break;  // If the player has all the miracles in the group, break

                var miracleId = list.RandomElement();
                rogue.AddMiracle(miracleId);

                list.Remove(i);  // Remove the miracle from the list so it can't be added again
            }
        }
    }
}
