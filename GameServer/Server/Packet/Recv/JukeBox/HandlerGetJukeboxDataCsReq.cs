using EggLink.DanhengServer.GameServer.Server.Packet.Send.JukeBox;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.JukeBox;

[Opcode(CmdIds.GetJukeboxDataCsReq)]
public class HandlerGetJukeboxDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetJukeboxDataScRsp(connection.Player!));
    }
}