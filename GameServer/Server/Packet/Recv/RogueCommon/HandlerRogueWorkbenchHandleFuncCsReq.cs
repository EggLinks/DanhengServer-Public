using EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.RogueCommon;

[Opcode(CmdIds.RogueWorkbenchHandleFuncCsReq)]
public class HandlerRogueWorkbenchHandleFuncCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = RogueWorkbenchHandleFuncCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        IGameEntity? entity = null;
        player.SceneInstance?.Entities.TryGetValue((int)req.PropEntityId, out entity);
        if (entity is not RogueWorkbenchProp prop)
        {
            await connection.SendPacket(new PacketRogueWorkbenchHandleFuncScRsp(Retcode.RetSceneEntityNotExist,
                req.WorkbenchFuncId, null));
            return;
        }

        var func = prop.WorkbenchFuncs.Find(x => x.FuncId == req.WorkbenchFuncId);
        if (func == null)
        {
            await connection.SendPacket(
                new PacketRogueWorkbenchHandleFuncScRsp(Retcode.RetFail, req.WorkbenchFuncId, null));
            return;
        }

        var instance = player.RogueTournManager?.RogueTournInstance;
        if (instance == null)
        {
            await connection.SendPacket(new PacketRogueWorkbenchHandleFuncScRsp(Retcode.RetTournRogueStatusMismatch,
                req.WorkbenchFuncId, null));
            return;
        }

        await instance.HandleFunc(func, req.WorkbenchContent);

        await connection.SendPacket(
            new PacketRogueWorkbenchHandleFuncScRsp(Retcode.RetSucc, req.WorkbenchFuncId, func));
    }
}