using EggLink.DanhengServer.GameServer.Game.Rogue.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketSyncRogueMapRoomScNotify : BasePacket
{
    public PacketSyncRogueMapRoomScNotify(RogueRoomInstance room, int mapId) : base(CmdIds.SyncRogueMapRoomScNotify)
    {
        var proto = new SyncRogueMapRoomScNotify
        {
            CurRoom = room.ToProto(),
            MapId = (uint)mapId
        };

        SetData(proto);
    }
}