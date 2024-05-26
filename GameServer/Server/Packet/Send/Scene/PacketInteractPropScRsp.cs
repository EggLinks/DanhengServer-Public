using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Scene
{
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
}
