using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSelectRogueDialogueEventScRsp : BasePacket
    {
        public PacketSelectRogueDialogueEventScRsp(RogueEventInstance rogueEvent) : base(CmdIds.SelectRogueDialogueEventScRsp)
        {
            var proto = new SelectRogueDialogueEventScRsp()
            {
                DialogueEventId = (uint)rogueEvent.EventId,
                ResultInfo = new(),
                EventInfo = rogueEvent.ToProto()
            };

            SetData(proto);
        }

        public PacketSelectRogueDialogueEventScRsp() : base(CmdIds.SelectRogueDialogueEventScRsp)
        {
            var proto = new SelectRogueDialogueEventScRsp()
            {
                Retcode = 1
            };

            SetData(proto);
        }
    }
}
