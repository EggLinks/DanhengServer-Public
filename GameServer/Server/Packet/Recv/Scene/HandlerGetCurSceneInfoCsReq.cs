using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.GetCurSceneInfoCsReq)]
public class HandlerGetCurSceneInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetCurSceneInfoScRsp(connection.Player!));
    }
}