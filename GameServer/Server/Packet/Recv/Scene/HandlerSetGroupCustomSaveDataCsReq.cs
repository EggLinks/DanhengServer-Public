using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.SetGroupCustomSaveDataCsReq)]
public class HandlerSetGroupCustomSaveDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SetGroupCustomSaveDataCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        player.SetCustomSaveData((int)req.EntryId, (int)req.GroupId, req.SaveData);
        await connection.SendPacket(new PacketSetGroupCustomSaveDataScRsp(req.EntryId, req.GroupId));
    }
}