using EggLink.DanhengServer.Game.Rogue.Event;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketSelectRogueCommonDialogueOptionScRsp : BasePacket
    {
        public PacketSelectRogueCommonDialogueOptionScRsp(RogueEventInstance rogueEvent) : base(CmdIds.SelectRogueCommonDialogueOptionScRsp)
        {
            var proto = new SelectRogueCommonDialogueOptionScRsp()
            {
                EventUniqueId = (uint)rogueEvent.EventUniqueId,
                DialogueData = rogueEvent.ToProto(),
                OptionId = (uint)rogueEvent.SelectedOptionId,
            };

            SetData(proto);
        }

        public PacketSelectRogueCommonDialogueOptionScRsp() : base(CmdIds.SelectRogueCommonDialogueOptionScRsp)
        {
            var proto = new SelectRogueCommonDialogueOptionScRsp()
            {
                Retcode = 1
            };

            SetData(proto);
        }
    }
}
