using EggLink.DanhengServer.GameServer.Game.RogueTourn;
using EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;

public class PacketRogueTournLevelInfoUpdateScNotify : BasePacket
{
    public PacketRogueTournLevelInfoUpdateScNotify(RogueTournInstance instance, List<RogueTournLevelInstance> levels) :
        base(CmdIds.RogueTournLevelInfoUpdateScNotify)
    {
        var proto = new RogueTournLevelInfoUpdateScNotify
        {
            Status = instance.LevelStatus,
            CurLevelIndex = (uint)(instance.CurLevel?.LevelIndex ?? 0),
            LevelInfoList = { levels.Select(x => x.ToProto()) }
        };

        SetData(proto);
    }
}