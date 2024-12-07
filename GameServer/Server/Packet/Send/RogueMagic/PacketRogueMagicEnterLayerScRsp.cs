using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;

public class PacketRogueMagicEnterLayerScRsp : BasePacket
{
    public PacketRogueMagicEnterLayerScRsp(Retcode ret, RogueMagicInstance? instance) : base(
        CmdIds.RogueMagicEnterLayerScRsp)
    {
        var proto = new RogueMagicEnterLayerScRsp
        {
            Retcode = (uint)ret
        };

        if (instance != null)
            proto.RogueTournCurSceneInfo = instance.ToCurSceneInfo();

        SetData(proto);
    }
}