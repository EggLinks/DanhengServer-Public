using EggLink.DanhengServer.GameServer.Server.Packet.Send.ServerPrefs;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ServerPrefs;

[Opcode(CmdIds.GetAllServerPrefsDataCsReq)]
public class HandlerGetAllServerPrefsDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var infos = connection.Player?.ServerPrefsData?.ServerPrefsDict.Values.ToList() ?? [];
        await connection.SendPacket(new PacketGetAllServerPrefsDataScRsp(infos));
    }
}