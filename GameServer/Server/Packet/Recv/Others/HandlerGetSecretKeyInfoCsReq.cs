using EggLink.DanhengServer.Server.Packet.Send.Others;

namespace EggLink.DanhengServer.Server.Packet.Recv.Others
{
    [Opcode(CmdIds.GetSecretKeyInfoCsReq)]
    public class HandlerGetSecretKeyInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetSecretKeyInfoScRsp());
        }
    }
}
