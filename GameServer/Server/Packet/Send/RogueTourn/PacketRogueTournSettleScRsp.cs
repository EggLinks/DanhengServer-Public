using EggLink.DanhengServer.GameServer.Game.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;

public class PacketRogueTournSettleScRsp : BasePacket
{
    public PacketRogueTournSettleScRsp(RogueTournInstance instance) : base(CmdIds.RogueTournSettleScRsp)
    {
        var proto = new RogueTournSettleScRsp
        {
            RogueTournCurSceneInfo = instance.ToCurSceneInfo(),
            TournFinishInfo = new RogueTournFinishInfo
            {
                RogueTournCurInfo = instance.ToProto(),
                RogueLineupInfo = instance.Player.LineupManager!.GetCurLineup()!.ToProto()
            }
        };

        SetData(proto);
    }

    public PacketRogueTournSettleScRsp() : base(CmdIds.RogueTournSettleScRsp)
    {
        var proto = new RogueTournSettleScRsp
        {
            Retcode = 1
        };

        SetData(proto);
    }
}