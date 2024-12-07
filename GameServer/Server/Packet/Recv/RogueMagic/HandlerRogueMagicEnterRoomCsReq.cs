using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueMagic;

[Opcode(CmdIds.RogueMagicEnterRoomCsReq)]
public class HandlerRogueMagicEnterRoomCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueMagicEnterRoomCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;

        var inst = player.RogueMagicManager?.RogueMagicInstance;
        if (inst == null)
        {
            await connection.SendPacket(new PacketRogueMagicEnterRoomScRsp(Retcode.RetRogueStatusNotMatch, null));
            return;
        }

        await inst.EnterRoom((int)(req.CurRoomIndex + 1), (RogueMagicRoomTypeEnum)req.NextRoomType);
        await connection.SendPacket(new PacketRogueMagicEnterRoomScRsp(Retcode.RetSucc, inst));
    }
}