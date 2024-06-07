using EggLink.DanhengServer.Server.Packet.Send.Avatar;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.GetHeroBasicTypeInfoCsReq)]
    public class HandlerGetHeroBasicTypeInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetHeroBasicTypeInfoScRsp(connection.Player!));
        }
    }
}
