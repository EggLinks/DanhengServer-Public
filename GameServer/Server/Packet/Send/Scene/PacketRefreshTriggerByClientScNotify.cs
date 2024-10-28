using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketRefreshTriggerByClientScNotify : BasePacket
{
    public PacketRefreshTriggerByClientScNotify(string triggerName, uint entityId, List<uint> targetIds) : base(
        CmdIds.RefreshTriggerByClientScNotify)
    {
        var rsp = new RefreshTriggerByClientScNotify
        {
            TriggerName = triggerName,
            TriggerEntityId = entityId,
            TriggerTargetIdList = { targetIds }
        };

        SetData(rsp);
    }
}