using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene;

[Opcode(CmdIds.SceneEntityMoveCsReq)]
public class HandlerSceneEntityMoveCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SceneEntityMoveCsReq.Parser.ParseFrom(data);
        if (req != null)
            foreach (var motion in req.EntityMotionList)
                if (connection.Player!.SceneInstance!.AvatarInfo.ContainsKey((int)motion.EntityId))
                {
                    connection.Player!.Data.Pos = motion.Motion.Pos.ToPosition();
                    connection.Player.Data.Rot = motion.Motion.Rot.ToPosition();
                    await connection.Player.OnMove();
                }

        await connection.SendPacket(CmdIds.SceneEntityMoveScRsp);
    }
}