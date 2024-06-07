using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Tutorial
{
    public class PacketUnlockTutorialGuideScRsp : BasePacket
    {
        public PacketUnlockTutorialGuideScRsp(uint tutorialId) : base(CmdIds.UnlockTutorialGuideScRsp)
        {
            var proto = new UnlockTutorialGuideScRsp
            {
                TutorialGuide = new()
                {
                    Id = tutorialId,
                    Status = TutorialStatus.TutorialUnlock,
                }
            };
            SetData(proto);
        }
    }
}
