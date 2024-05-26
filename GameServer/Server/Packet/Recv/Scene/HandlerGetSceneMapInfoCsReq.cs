using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Scene;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.GetSceneMapInfoCsReq)]
    public class HandlerGetSceneMapInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetSceneMapInfoCsReq.Parser.ParseFrom(data);
            connection.SendPacket(new PacketGetSceneMapInfoScRsp(req, connection.Player!));
        }
    }
}
