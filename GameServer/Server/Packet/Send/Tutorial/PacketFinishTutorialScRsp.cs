using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Tutorial
{
    public class PacketFinishTutorialScRsp : BasePacket
    {
        public PacketFinishTutorialScRsp(uint tutorialId) : base(CmdIds.FinishTutorialScRsp)
        {
            var rsp = new FinishTutorialScRsp
            {
                Tutorial = new Proto.Tutorial
                {
                    Id = tutorialId,
                    Status = TutorialStatus.TutorialFinish,
                },
            };

            SetData(rsp);
        }
    }
}
