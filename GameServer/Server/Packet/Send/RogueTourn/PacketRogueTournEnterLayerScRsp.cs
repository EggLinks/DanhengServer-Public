using EggLink.DanhengServer.GameServer.Game.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;

public class PacketRogueTournEnterLayerScRsp : BasePacket
{
    public PacketRogueTournEnterLayerScRsp(Retcode ret, RogueTournInstance? instance) : base(
        CmdIds.RogueTournEnterLayerScRsp)
    {
        var proto = new RogueTournEnterLayerScRsp
        {
            Retcode = (uint)ret
        };

        if (instance != null)
            proto.RogueTournCurSceneInfo = instance.ToCurSceneInfo();

        SetData(proto);
    }
}