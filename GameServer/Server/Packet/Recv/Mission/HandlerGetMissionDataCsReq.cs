using EggLink.DanhengServer.Server.Packet.Send.Mission;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.GetMissionDataCsReq)]
    public class HandlerGetMissionDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetMissionDataScRsp(connection.Player!));
        }
    }
}
