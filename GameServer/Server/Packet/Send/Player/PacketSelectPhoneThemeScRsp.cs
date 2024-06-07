using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSelectPhoneThemeScRsp : BasePacket
    {
        public PacketSelectPhoneThemeScRsp(uint themeId) : base(CmdIds.SelectPhoneThemeScRsp)
        {
            var proto = new SelectPhoneThemeScRsp
            {
                CurPhoneTheme = themeId
            };

            SetData(proto);
        }
    }
}
