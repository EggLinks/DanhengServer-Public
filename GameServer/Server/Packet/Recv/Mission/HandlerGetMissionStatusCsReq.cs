using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.GetMissionStatusCsReq)]
    public class HandlerGetMissionStatusCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetMissionStatusCsReq.Parser.ParseFrom(data);
            if (req != null)
            {
                connection.SendPacket(new PacketGetMissionStatusScRsp(req, connection.Player!));
            }
        }
    }
}
