using EggLink.DanhengServer.GameServer.Server.Packet.Send.ServerPrefs;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ServerPrefs;

[Opcode(CmdIds.GetServerPrefsDataCsReq)]
public class HandlerGetServerPrefsDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetServerPrefsDataCsReq.Parser.ParseFrom(data);

        var info = connection.Player!.ServerPrefsData?.ServerPrefsDict.GetValueOrDefault((int)req.ServerPrefsId);

        await connection.SendPacket(new PacketGetServerPrefsDataScRsp(info, req.ServerPrefsId));
    }
}