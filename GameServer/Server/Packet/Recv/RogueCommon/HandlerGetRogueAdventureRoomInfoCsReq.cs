using EggLink.DanhengServer.GameServer.Game.RogueMagic;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueCommon;

[Opcode(CmdIds.GetRogueAdventureRoomInfoCsReq)]
public class HandlerGetRogueAdventureRoomInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var instance = connection.Player?.RogueManager?.GetRogueInstance();

        if (instance == null)
        {
            await connection.SendPacket(new PacketGetRogueAdventureRoomInfoScRsp(Retcode.RetAdventureMapNotExist));
            return;
        }

        if (instance is not RogueMagicInstance magic)
        {
            await connection.SendPacket(new PacketGetRogueAdventureRoomInfoScRsp(Retcode.RetAdventureMapNotExist));
            return;
        }

        var inst = magic.CurLevel?.CurRoom?.AdventureInstance;
        if (inst == null)
        {
            await connection.SendPacket(new PacketGetRogueAdventureRoomInfoScRsp(Retcode.RetAdventureMapNotExist));
            return;
        }

        await connection.SendPacket(new PacketGetRogueAdventureRoomInfoScRsp(inst));
    }
}