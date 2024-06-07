using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSelectChatBubbleScRsp : BasePacket
    {
        public PacketSelectChatBubbleScRsp(uint bubbleId) : base(CmdIds.SelectChatBubbleScRsp)
        {
            var proto = new SelectChatBubbleScRsp
            {
                CurChatBubble = bubbleId
            };

            SetData(proto);
        }
    }
}
