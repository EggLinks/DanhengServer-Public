using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Tutorial;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Tutorial;

[Opcode(CmdIds.UnlockTutorialGuideCsReq)]
public class HandlerUnlockTutorialGuideCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = UnlockTutorialGuideCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;
        if (!player.TutorialGuideData!.Tutorials.TryGetValue((int)req.GroupId, out var _))
        {
            player.TutorialGuideData!.Tutorials.Add((int)req.GroupId, TutorialStatus.TutorialUnlock);
            DatabaseHelper.Instance?.UpdateInstance(player.TutorialGuideData!);
        }

        await connection.SendPacket(new PacketUnlockTutorialGuideScRsp(req.GroupId));
    }
}