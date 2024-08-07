using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.GetSceneMapInfoCsReq)]
public class HandlerGetSceneMapInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetSceneMapInfoCsReq.Parser.ParseFrom(data);
        await connection.SendPacket(new PacketGetSceneMapInfoScRsp(req, connection.Player!));
    }
}