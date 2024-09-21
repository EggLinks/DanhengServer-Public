using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketStartRogueScRsp : BasePacket
{
    public PacketStartRogueScRsp(PlayerInstance player) : base(CmdIds.StartRogueScRsp)
    {
        var proto = new StartRogueScRsp
        {
            RogueGameInfo = player.RogueManager!.ToProto(),
            Lineup = player.LineupManager!.GetCurLineup()!.ToProto(),
            Scene = player.SceneInstance!.ToProto()
        };

        SetData(proto);
    }
}