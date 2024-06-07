using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSetSignatureScRsp : BasePacket
    {
        public PacketSetSignatureScRsp(string signature) : base(CmdIds.SetSignatureScRsp)
        {
            var proto = new SetSignatureScRsp()
            {
                Signature = signature
            };

            SetData(proto);
        }
    }
}
