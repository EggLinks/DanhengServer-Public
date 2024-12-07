using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using Google.Protobuf;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;

public class PacketRogueMagicStartScRsp : BasePacket
{
    public PacketRogueMagicStartScRsp(Retcode retcode, RogueMagicInstance? instance) : base(CmdIds.RogueMagicStartScRsp)
    {
        var rsp = new RogueMagicStartScRsp
        {
            Retcode = (uint)retcode
        };

        if (instance != null)
        {
            rsp.RogueTournCurSceneInfo = instance.ToCurSceneInfo();
            rsp.RogueTournCurInfo = instance.ToProto();
        }

        Data = rsp.ToByteArray();
    }
}