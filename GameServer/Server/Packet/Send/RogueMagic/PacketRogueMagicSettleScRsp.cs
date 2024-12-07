using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;

public class PacketRogueMagicSettleScRsp : BasePacket
{
    public PacketRogueMagicSettleScRsp(RogueMagicInstance instance) : base(CmdIds.RogueMagicSettleScRsp)
    {
        var proto = new RogueMagicSettleScRsp
        {
            RogueTournCurSceneInfo = instance.ToCurSceneInfo(),
            TournFinishInfo = new RogueMagicFinishInfo
            {
                RogueTournCurInfo = instance.ToProto(),
                RogueLineupInfo = instance.Player.LineupManager!.GetCurLineup()!.ToProto()
            }
        };

        SetData(proto);
    }

    public PacketRogueMagicSettleScRsp() : base(CmdIds.RogueMagicSettleScRsp)
    {
        var proto = new RogueMagicSettleScRsp
        {
            Retcode = 1
        };

        SetData(proto);
    }
}