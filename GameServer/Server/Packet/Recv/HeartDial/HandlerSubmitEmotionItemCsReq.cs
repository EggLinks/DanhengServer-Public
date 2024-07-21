using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Mission;
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
    [Opcode(CmdIds.SubmitEmotionItemCsReq)]
    public class HandlerSubmitEmotionItemCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SubmitEmotionItemCsReq.Parser.ParseFrom(data);

            GameData.HeartDialScriptData.TryGetValue((int)req.ScriptId, out var scriptData);
            if (scriptData != null)
            {
                var info = connection.Player!.HeartDialData!.ChangeScriptEmotion((int)req.ScriptId, scriptData.MissingEmoList[0], HeartDialStepTypeEnum.Normal);
                connection.Player!.SendPacket(new PacketHeartDialScriptChangeScNotify(HeartDialUnlockStatus.UnlockAll, info));
                connection.Player!.MissionManager?.HandleFinishType(Enums.MissionFinishTypeEnum.HeartDialScriptListStep);
            }

            connection.SendPacket(new PacketSubmitEmotionItemScRsp(req.ScriptId));
        }
    }
}
