using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial
{
    public class PacketChangeScriptEmotionScRsp : BasePacket
    {
        public PacketChangeScriptEmotionScRsp(uint scriptId, HeartDialEmotionType emotion) : base(CmdIds.ChangeScriptEmotionScRsp)
        {
            var proto = new ChangeScriptEmotionScRsp
            {
                ScriptId = scriptId,
                EmotionType = emotion
            };

            SetData(proto);
        }
    }
}
