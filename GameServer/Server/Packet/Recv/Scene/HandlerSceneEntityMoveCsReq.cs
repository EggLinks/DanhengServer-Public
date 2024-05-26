using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.SceneEntityMoveCsReq)]
    public class HandlerSceneEntityMoveCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SceneEntityMoveCsReq.Parser.ParseFrom(data);
            if (req != null)
            {
                foreach (var motion in req.EntityMotionList) 
                {
                    if (connection.Player!.SceneInstance!.AvatarInfo.ContainsKey((int)motion.EntityId))
                    {
                        connection.Player!.Data.Pos = motion.Motion.Pos.ToPosition();
                        connection.Player.Data.Rot = motion.Motion.Rot.ToPosition();
                        connection.Player.OnMove();
                    }
                }
            }

            connection.SendPacket(CmdIds.SceneEntityMoveScRsp);
        }
    }
}
