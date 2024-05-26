using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.EnterSectionCsReq)]
    public class HandlerEnterSectionCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = EnterSectionCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            player.EnterSection((int)req.SectionId);
            connection.SendPacket(CmdIds.EnterSectionScRsp);
        }
    }
}
