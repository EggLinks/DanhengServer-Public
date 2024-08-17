using EggLink.DanhengServer.GameServer.Server.Packet.Send.ServerPrefs;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.ServerPrefs;

[Opcode(CmdIds.UpdateServerPrefsDataCsReq)]
public class HandlerUpdateServerPrefsDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = UpdateServerPrefsDataCsReq.Parser.ParseFrom(data);

        connection.Player?.ServerPrefsData?.SetData((int)req.ServerPrefs.ServerPrefsId,
            req.ServerPrefs.Data.ToBase64());
        await connection.SendPacket(new PacketUpdateServerPrefsDataScRsp(req.ServerPrefs.ServerPrefsId));
    }
}