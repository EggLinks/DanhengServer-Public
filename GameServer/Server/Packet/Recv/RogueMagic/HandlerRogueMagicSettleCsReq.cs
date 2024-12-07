using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueMagic;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueMagic;

[Opcode(CmdIds.RogueMagicSettleCsReq)]
public class HandlerRogueMagicSettleCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var inst = connection.Player!.RogueMagicManager?.RogueMagicInstance;

        if (inst == null)
        {
            await connection.SendPacket(new PacketRogueMagicSettleScRsp());
            return;
        }

        await inst.QuitRogue();
        await connection.SendPacket(new PacketRogueMagicSettleScRsp(inst));
    }
}