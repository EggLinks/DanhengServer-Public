using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Tutorial
{
    public class PacketUnlockTutorialScRsp : BasePacket
    {
        public PacketUnlockTutorialScRsp(uint tutorialId) : base(CmdIds.UnlockTutorialScRsp)
        {
            var proto = new UnlockTutorialScRsp
            {
                Tutorial = new()
                {
                    Id = tutorialId,
                    Status = TutorialStatus.TutorialUnlock,
                }
            };
            SetData(proto);
        }
    }
}
