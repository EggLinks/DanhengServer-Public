using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketEnhanceRogueBuffScRsp : BasePacket
    {
        public PacketEnhanceRogueBuffScRsp(uint buffId) : base(CmdIds.EnhanceRogueBuffScRsp)
        {
            var proto = new EnhanceRogueBuffScRsp
            {
                RogueBuff = new()
                {
                    BuffId = buffId,
                    Level = 2
                },
                IsSuccess = true
            };

            SetData(proto);
        }
    }
}
