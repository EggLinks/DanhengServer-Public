using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueCommonDialogueDataScNotify : BasePacket
    {
        public PacketSyncRogueCommonDialogueDataScNotify(RogueEventInstance rogueEvent) : base(CmdIds.SyncRogueCommonDialogueDataScNotify)
        {
            var proto = new SyncRogueCommonDialogueDataScNotify();

            proto.DialogueEventList.Add(rogueEvent.ToProto());

            SetData(proto);
        }
    }
}
