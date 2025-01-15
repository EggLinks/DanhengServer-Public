using EggLink.DanhengServer.GameServer.Server.Packet.Send.PamSkin;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.PamSkin;

[Opcode(CmdIds.GetPamSkinDataCsReq)]
public class HandlerGetPamSkinDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketGetPamSkinDataScRsp(connection.Player!));
    }
}