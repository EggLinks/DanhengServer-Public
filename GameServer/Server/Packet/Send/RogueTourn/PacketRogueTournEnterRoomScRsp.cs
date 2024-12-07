using EggLink.DanhengServer.GameServer.Game.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;

public class PacketRogueTournEnterRoomScRsp : BasePacket
{
    public PacketRogueTournEnterRoomScRsp(Retcode ret, RogueTournInstance? instance) : base(
        CmdIds.RogueTournEnterRoomScRsp)
    {
        var proto = new RogueTournEnterRoomScRsp
        {
            Retcode = (uint)ret
        };

        if (instance != null)
            proto.RogueTournCurSceneInfo = instance.ToCurSceneInfo();

        SetData(proto);
    }
}