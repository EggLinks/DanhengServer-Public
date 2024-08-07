using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketSyncRogueCommonDialogueDataScNotify : BasePacket
{
    public PacketSyncRogueCommonDialogueDataScNotify(RogueEventInstance rogueEvent) : base(
        CmdIds.SyncRogueCommonDialogueDataScNotify)
    {
        var proto = new SyncRogueCommonDialogueDataScNotify();

        proto.DialogueDataList.Add(rogueEvent.ToProto());

        SetData(proto);
    }
}