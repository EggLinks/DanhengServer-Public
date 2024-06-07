using EggLink.DanhengServer.Server.Packet.Send.Battle;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.GetChallengeCsReq)]
    public class HandlerGetChallengeCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetChallengeScRsp(connection.Player!));
        }
    }
}
