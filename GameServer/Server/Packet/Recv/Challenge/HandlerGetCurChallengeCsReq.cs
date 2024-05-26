using EggLink.DanhengServer.Server.Packet.Send.Battle;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.GetCurChallengeCsReq)]
    public class HandlerGetCurChallengeCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            // Send packet first
            connection.SendPacket(new PacketGetCurChallengeScRsp(connection.Player!));

            // Update data
            if (connection.Player!.ChallengeManager!.ChallengeInstance != null)
            {
                connection.Player.ChallengeManager.ChallengeInstance.OnUpdate();
            }
        }
    }
}
