using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSetHeroBasicTypeScRsp : BasePacket
    {
        public PacketSetHeroBasicTypeScRsp(uint basicType) : base(CmdIds.SetHeroBasicTypeScRsp)
        {
            var proto = new SetHeroBasicTypeScRsp
            {
                BasicType = (HeroBasicType)basicType
            };

            SetData(proto);
        }

        public PacketSetHeroBasicTypeScRsp() : base(CmdIds.SetHeroBasicTypeScRsp)
        {
            var proto = new SetHeroBasicTypeScRsp
            {
                Retcode = 1
            };

            SetData(proto);
        }
    }
}
