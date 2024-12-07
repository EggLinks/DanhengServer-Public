using EggLink.DanhengServer.Enums.RogueMagic;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueMagic;

[Opcode(CmdIds.RogueMagicEnterLayerCsReq)]
public class HandlerRogueMagicEnterLayerCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueMagicEnterLayerCsReq.Parser.ParseFrom(data);
        var inst = connection.Player!.RogueMagicManager?.RogueMagicInstance;

        if (inst == null)
        {
            await connection.SendPacket(new PacketRogueMagicEnterLayerScRsp(Retcode.RetRogueStatusNotMatch, null));
            return;
        }

        await inst.EnterNextLayer((int)req.CurLevelIndex, (RogueMagicRoomTypeEnum)req.NextRoomType);
        await connection.SendPacket(new PacketRogueMagicEnterLayerScRsp(Retcode.RetSucc, inst));
    }
}