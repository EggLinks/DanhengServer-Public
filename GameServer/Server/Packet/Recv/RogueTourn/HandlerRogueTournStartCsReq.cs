using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueTourn;

[Opcode(CmdIds.RogueTournStartCsReq)]
public class HandlerRogueTournStartCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueTournStartCsReq.Parser.ParseFrom(data);

        var avatars = req.BaseAvatarIdList.Select(x => (int)x);
        var area = (int)req.AreaId;
        var week = (int)req.Week;
        var difficulty = req.StartDifficultyIdList.Select(x => (int)x);

        var player = connection.Player!;
        var rsp = await player.RogueTournManager!.StartRogueTourn(avatars.ToList(), area, week, difficulty.ToList());
        await connection.SendPacket(new PacketRogueTournStartScRsp(rsp.Item1, rsp.Item2));
    }
}