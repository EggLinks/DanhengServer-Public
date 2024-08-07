using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial;

[Opcode(CmdIds.GetHeartDialInfoCsReq)]
public class HandlerGetHeartDialInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetHeartDialInfoScRsp(connection.Player!));
    }
}