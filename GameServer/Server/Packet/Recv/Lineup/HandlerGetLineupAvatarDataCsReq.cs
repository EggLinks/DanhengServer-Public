using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Lineup;

namespace EggLink.DanhengServer.Server.Packet.Recv.Lineup
{
    [Opcode(CmdIds.GetLineupAvatarDataCsReq)]
    public class HandlerGetLineupAvatarDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetLineupAvatarDataScRsp(connection.Player!));
        }
    }
}
