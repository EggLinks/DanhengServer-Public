using EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueCommon;

[Opcode(CmdIds.RogueWorkbenchGetInfoCsReq)]
public class HandlerRogueWorkbenchGetInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueWorkbenchGetInfoCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        IGameEntity? entity = null;
        player.SceneInstance?.Entities.TryGetValue((int)req.PropEntityId, out entity);
        if (entity is not RogueWorkbenchProp prop)
        {
            await connection.SendPacket(new PacketRogueWorkbenchGetInfoScRsp(Retcode.RetSceneEntityNotExist, null));
            return;
        }

        await connection.SendPacket(new PacketRogueWorkbenchGetInfoScRsp(Retcode.RetSucc, prop));
    }
}