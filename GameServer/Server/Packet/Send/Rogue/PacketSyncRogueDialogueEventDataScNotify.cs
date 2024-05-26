using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueDialogueEventDataScNotify : BasePacket
    {
        public PacketSyncRogueDialogueEventDataScNotify(RogueEventInstance rogueEvent) : base(CmdIds.SyncRogueDialogueEventDataScNotify)
        {
            var proto = new SyncRogueDialogueEventDataScNotify();

            proto.DialogueEventList.Add(rogueEvent.ToProto());

            SetData(proto);
        }
    }
}
