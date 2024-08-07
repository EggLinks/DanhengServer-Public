using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event.CostHandler;

[RogueEvent(costType: DialogueEventCostTypeEnum.CostHpCurrentPercent)]
public class EventHandlerCostHpCurrentPercent : RogueEventCostHandler
{
    public override async ValueTask Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance,
        List<int> paramList)
    {
        if (rogue.CurLineup!.CostNowPercentHp(paramList[0] / 100f))
            // sync
            await rogue.Player!.SendPacket(new PacketSyncLineupNotify(rogue.CurLineup!));
    }
}