using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.GetPlayerBoardDataCsReq)]
    public class HandlerGetPlayerBoardDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetPlayerBoardDataScRsp(connection.Player!));
        }
    }
}
