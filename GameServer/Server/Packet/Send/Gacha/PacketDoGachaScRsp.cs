using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Gacha
{
    public class PacketDoGachaScRsp : BasePacket
    {
        public PacketDoGachaScRsp(DoGachaScRsp rsp) : base(CmdIds.DoGachaScRsp)
        {
            SetData(rsp);
        }

        public PacketDoGachaScRsp() : base(CmdIds.DoGachaScRsp)
        {
            var rsp = new DoGachaScRsp
            {
                Retcode = 1,
            };
            SetData(rsp);
        }
    }
}
