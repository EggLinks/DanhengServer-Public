using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketInteractPropScRsp : BasePacket
{
    public PacketInteractPropScRsp(EntityProp? prop) : base(CmdIds.InteractPropScRsp)
    {
        var proto = new InteractPropScRsp();

        if (prop != null)
        {
            proto.PropState = (uint)prop.State;
            proto.PropEntityId = (uint)prop.EntityID;
        }

        SetData(proto);
    }
}