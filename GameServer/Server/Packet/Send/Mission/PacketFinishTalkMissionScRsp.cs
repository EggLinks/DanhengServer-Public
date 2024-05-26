using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketFinishTalkMissionScRsp : BasePacket
    {
        public PacketFinishTalkMissionScRsp(string talkStr) : base(CmdIds.FinishTalkMissionScRsp)
        {
            var proto = new FinishTalkMissionScRsp()
            {
                TalkStr = talkStr,
            };

            SetData(proto);
        }
    }
}
