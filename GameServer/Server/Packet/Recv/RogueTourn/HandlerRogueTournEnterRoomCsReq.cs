using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueTourn;

[Opcode(CmdIds.RogueTournEnterRoomCsReq)]
public class HandlerRogueTournEnterRoomCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueTournEnterRoomCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;

        var inst = player.RogueTournManager?.RogueTournInstance;
        if (inst == null)
        {
            await connection.SendPacket(new PacketRogueTournEnterRoomScRsp(Retcode.RetRogueStatusNotMatch, null));
            return;
        }

        await inst.EnterRoom((int)(req.CurRoomIndex + 1), (RogueTournRoomTypeEnum)req.NextRoomType);
        await connection.SendPacket(new PacketRogueTournEnterRoomScRsp(Retcode.RetSucc, inst));
    }
}