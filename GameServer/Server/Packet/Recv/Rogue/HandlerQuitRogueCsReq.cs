using EggLink.DanhengServer.Server.Packet.Send.Rogue;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.QuitRogueCsReq)]
    public class HandlerQuitRogueCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.Player!.RogueManager!.RogueInstance?.QuitRogue();
            connection.SendPacket(new PacketQuitRogueScRsp(connection.Player!));
        }
    }
}
