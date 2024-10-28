using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketSyncRogueCommonPendingActionScNotify : BasePacket
{
    public PacketSyncRogueCommonPendingActionScNotify(RogueActionInstance actionInstance, RogueSubModeEnum rogueSubmode)
        : base(
            CmdIds.SyncRogueCommonPendingActionScNotify)
    {
        var proto = new SyncRogueCommonPendingActionScNotify
        {
            Action = actionInstance.ToProto(),
            RogueSubMode = (uint)rogueSubmode
        };

        SetData(proto);
    }
}