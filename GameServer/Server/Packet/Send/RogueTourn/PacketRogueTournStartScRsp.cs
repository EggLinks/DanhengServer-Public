using EggLink.DanhengServer.GameServer.Game.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;

public class PacketRogueTournStartScRsp : BasePacket
{
    public PacketRogueTournStartScRsp(Retcode retcode, RogueTournInstance? rogueTourn) : base(
        CmdIds.RogueTournStartScRsp)
    {
        var proto = new RogueTournStartScRsp
        {
            Retcode = (uint)retcode
        };

        if (rogueTourn != null)
        {
            proto.RogueTournCurInfo = rogueTourn.ToProto();
            proto.RogueTournCurSceneInfo = rogueTourn.ToCurSceneInfo();
        }

        SetData(proto);
    }
}