using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Scene;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.InteractPropCsReq)]
    public class HandlerInteractPropCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = InteractPropCsReq.Parser.ParseFrom(data);
            var prop = connection.Player?.InteractProp((int)req.PropEntityId, (int)req.InteractId);
            connection.SendPacket(new PacketInteractPropScRsp(prop));
        }
    }
}
