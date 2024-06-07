using EggLink.DanhengServer.Server.Packet.Send.Lineup;

namespace EggLink.DanhengServer.Server.Packet.Recv.Lineup
{
    [Opcode(CmdIds.GetAllLineupDataCsReq)]
    public class HandlerGetAllLineupDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetAllLineupDataScRsp(connection.Player!));
        }
    }
}
