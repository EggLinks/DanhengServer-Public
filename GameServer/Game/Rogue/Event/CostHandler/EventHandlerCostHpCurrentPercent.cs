using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event.CostHandler
{
    [RogueEvent(costType: DialogueEventCostTypeEnum.CostHpCurrentPercent)]
    public class EventHandlerCostHpCurrentPercent : RogueEventCostHandler
    {
        public override void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList)
        {
            if (rogue.CurLineup!.CostNowPercentHp(ParamList[0] / 100f))
            {
                // sync
                rogue.Player!.SendPacket(new PacketSyncLineupNotify(rogue.CurLineup!));
            }
        }
    }
}
