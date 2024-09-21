using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketLeaveRogueScRsp : BasePacket
{
    public PacketLeaveRogueScRsp(PlayerInstance player) : base(CmdIds.LeaveRogueScRsp)
    {
        var proto = new LeaveRogueScRsp
        {
            Lineup = player.LineupManager!.GetCurLineup()!.ToProto(),
            Scene = player.SceneInstance!.ToProto(),
            RogueGameInfo = player.RogueManager!.ToProto()
        };

        SetData(proto);
    }
}