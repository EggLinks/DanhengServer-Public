using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial
{
    public class PacketSubmitEmotionItemScRsp : BasePacket
    {
        public PacketSubmitEmotionItemScRsp(uint scriptId) : base(CmdIds.SubmitEmotionItemScRsp)
        {
            var proto = new SubmitEmotionItemScRsp
            {
                ScriptId = scriptId
            };

            SetData(proto);
        }
    }
}
