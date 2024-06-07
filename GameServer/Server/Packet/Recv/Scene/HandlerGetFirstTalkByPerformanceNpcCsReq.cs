using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Scene;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.GetFirstTalkByPerformanceNpcCsReq)]
    public class HandlerGetFirstTalkByPerformanceNpcCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetFirstTalkByPerformanceNpcCsReq.Parser.ParseFrom(data);
            connection.SendPacket(new PacketGetFirstTalkByPerformanceNpcScRsp(req));
        }
    }
}
