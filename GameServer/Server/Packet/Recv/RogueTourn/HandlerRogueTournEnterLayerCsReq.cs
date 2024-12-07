using EggLink.DanhengServer.Enums.TournRogue;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueTourn;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueTourn;

[Opcode(CmdIds.RogueTournEnterLayerCsReq)]
public class HandlerRogueTournEnterLayerCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueTournEnterLayerCsReq.Parser.ParseFrom(data);
        var inst = connection.Player!.RogueTournManager?.RogueTournInstance;

        if (inst == null)
        {
            await connection.SendPacket(new PacketRogueTournEnterLayerScRsp(Retcode.RetRogueStatusNotMatch, null));
            return;
        }

        await inst.EnterNextLayer((int)req.CurLevelIndex, (RogueTournRoomTypeEnum)req.NextRoomType);
        await connection.SendPacket(new PacketRogueTournEnterLayerScRsp(Retcode.RetSucc, inst));
    }
}