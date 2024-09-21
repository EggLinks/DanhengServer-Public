using EggLink.DanhengServer.GameServer.Server.Packet.Send.Tutorial;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Tutorial;

[Opcode(CmdIds.UnlockTutorialCsReq)]
public class HandlerUnlockTutorialCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = UnlockTutorialCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        if (!player.TutorialData!.Tutorials.TryGetValue((int)req.TutorialId, out var _))
            player.TutorialData!.Tutorials.Add((int)req.TutorialId, TutorialStatus.TutorialUnlock);

        await connection.SendPacket(new PacketUnlockTutorialScRsp(req.TutorialId));
    }
}