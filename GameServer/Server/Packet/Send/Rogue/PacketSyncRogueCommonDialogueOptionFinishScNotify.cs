using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue
{
    public class PacketSyncRogueCommonDialogueOptionFinishScNotify : BasePacket
    {
        public PacketSyncRogueCommonDialogueOptionFinishScNotify(RogueEventInstance instance) : base(CmdIds.SyncRogueCommonDialogueOptionFinishScNotify)
        {
            var proto = new SyncRogueCommonDialogueOptionFinishScNotify()
            {
                DialogueData = instance.ToProto(),
                EventUniqueId = (uint)instance.EventUniqueId,
                OptionId = (uint)instance.SelectedOptionId,
                ResultOptionInfo = instance.Options.Find(o => o.OptionId == instance.SelectedOptionId)!.ToProto()
            };

            SetData(proto);
        }
    }
}
