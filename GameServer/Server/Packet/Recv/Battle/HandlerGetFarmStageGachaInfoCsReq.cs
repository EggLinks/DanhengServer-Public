using EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Battle;

[Opcode(CmdIds.GetFarmStageGachaInfoCsReq)]
public class HandlerGetFarmStageGachaInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetFarmStageGachaInfoCsReq.Parser.ParseFrom(data);
        await connection.SendPacket(new PacketGetFarmStageGachaInfoScRsp(req));
    }
}