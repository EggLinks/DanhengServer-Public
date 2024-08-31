using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.RefreshTriggerByClientCsReq)]
public class HandlerRefreshTriggerByClientCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RefreshTriggerByClientCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;
        var ret = await player.SceneInstance!.TriggerSummonUnit(req.TriggerName, req.TriggerTargetIdList.ToList());

        await connection.SendPacket(new PacketRefreshTriggerByClientScRsp(ret, req.TriggerName, req.TriggerEntityId));
    }
}