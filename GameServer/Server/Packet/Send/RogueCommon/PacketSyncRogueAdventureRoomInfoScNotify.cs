using EggLink.DanhengServer.GameServer.Game.RogueMagic.Adventure;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketSyncRogueAdventureRoomInfoScNotify : BasePacket
{
    public PacketSyncRogueAdventureRoomInfoScNotify(RogueMagicAdventureInstance instance) : base(
        CmdIds.SyncRogueAdventureRoomInfoScNotify)
    {
        var proto = new SyncRogueAdventureRoomInfoScNotify
        {
            AdventureRoomInfo = instance.ToProto()
        };

        SetData(proto);
    }
}