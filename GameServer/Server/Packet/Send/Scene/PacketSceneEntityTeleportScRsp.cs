using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneEntityTeleportScRsp : BasePacket
{
    public PacketSceneEntityTeleportScRsp(EntityMotion motion) : base(CmdIds.SceneEntityTeleportScRsp)
    {
        var proto = new SceneEntityTeleportScRsp
        {
            EntityMotion = motion
        };

        SetData(proto);
    }
}