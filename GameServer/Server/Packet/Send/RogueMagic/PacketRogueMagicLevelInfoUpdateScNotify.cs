using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.GameServer.Game.RogueMagic.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;

public class PacketRogueMagicLevelInfoUpdateScNotify : BasePacket
{
    public PacketRogueMagicLevelInfoUpdateScNotify(RogueMagicInstance instance, List<RogueMagicLevelInstance> levels,
        List<int>? updateRoomIndexList = null) : base(CmdIds.RogueMagicLevelInfoUpdateScNotify)
    {
        var proto = new RogueMagicLevelInfoUpdateScNotify
        {
            Status = instance.LevelStatus,
            CurLevelIndex = (uint)(instance.CurLevel?.LevelIndex ?? 0),
            LevelInfoList = { levels.Select(x => x.ToProto(updateRoomIndexList)) }
        };

        SetData(proto);
    }
}