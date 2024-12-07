using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;

public class PacketRogueMagicEnterRoomScRsp : BasePacket
{
    public PacketRogueMagicEnterRoomScRsp(Retcode ret, RogueMagicInstance? instance) : base(
        CmdIds.RogueMagicEnterRoomScRsp)
    {
        var proto = new RogueMagicEnterRoomScRsp
        {
            Retcode = (uint)ret
        };

        if (instance != null)
            proto.RogueTournCurSceneInfo = instance.ToCurSceneInfo();

        SetData(proto);
    }
}