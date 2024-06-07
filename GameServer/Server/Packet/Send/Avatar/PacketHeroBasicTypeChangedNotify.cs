using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketHeroBasicTypeChangedNotify : BasePacket
    {
        public PacketHeroBasicTypeChangedNotify(int type) : base(CmdIds.HeroBasicTypeChangedNotify)
        {
            var proto = new HeroBasicTypeChangedNotify()
            {
                CurBasicType = (HeroBasicType)type
            };

            SetData(proto);
        }
    }
}
