using EggLink.DanhengServer.Server.Packet.Send.Scene;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.GetCurSceneInfoCsReq)]
    public class HandlerGetCurSceneInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetCurSceneInfoScRsp(connection.Player!));
        }
    }
}
