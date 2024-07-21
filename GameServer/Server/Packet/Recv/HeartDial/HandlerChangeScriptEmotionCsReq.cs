using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial
{
    [Opcode(CmdIds.ChangeScriptEmotionCsReq)]
    public class HandlerChangeScriptEmotionCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ChangeScriptEmotionCsReq.Parser.ParseFrom(data);

            connection.Player!.HeartDialData!.ChangeScriptEmotion((int)req.ScriptId, (Enums.Mission.HeartDialEmoTypeEnum)req.TargetEmotionType);

            connection.SendPacket(new PacketChangeScriptEmotionScRsp(req.ScriptId, req.TargetEmotionType));
        }
    }
}
