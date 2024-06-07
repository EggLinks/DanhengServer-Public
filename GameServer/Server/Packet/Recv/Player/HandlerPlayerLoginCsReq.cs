using EggLink.DanhengServer.Common.Enums;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.PlayerLoginCsReq)]
    public class HandlerPlayerLoginCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.State = SessionState.ACTIVE;
            connection.SendPacket(new PacketPlayerLoginScRsp(connection));
        }
    }
}
