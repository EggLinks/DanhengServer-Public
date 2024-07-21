using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial
{
    public class PacketFinishEmotionDialoguePerformanceScRsp : BasePacket
    {
        public PacketFinishEmotionDialoguePerformanceScRsp(uint scriptId, uint dialogueId) : base(CmdIds.FinishEmotionDialoguePerformanceScRsp)
        {
            var proto = new FinishEmotionDialoguePerformanceScRsp
            {
                DialogueId = dialogueId,
                ScriptId = scriptId,
                RewardList = new()
            };

            SetData(proto);
        }
    }
}
