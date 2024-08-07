using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Lineup;

[Opcode(CmdIds.SwitchLineupIndexCsReq)]
public class HandlerSwitchLineupIndexCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SwitchLineupIndexCsReq.Parser.ParseFrom(data);
        if (await connection.Player!.LineupManager!
                .SetCurLineup((int)req.Index)) // SetCurLineup returns true if the index is valid
            await connection.SendPacket(new PacketSwitchLineupIndexScRsp(req.Index));
    }
}