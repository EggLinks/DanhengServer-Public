using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Phone;

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