using EggLink.DanhengServer.GameServer.Server.Packet.Send.TrainParty;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.TrainParty;

[Opcode(CmdIds.TrainPartyAddBuildDynamicBuffCsReq)]
public class HandlerTrainPartyAddBuildDynamicBuffCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketTrainPartyAddBuildDynamicBuffScRsp());
    }
}