using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SelectPhoneThemeCsReq)]
    public class HandlerSelectPhoneThemeCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SelectPhoneThemeCsReq.Parser.ParseFrom(data);

            connection.Player!.Data.PhoneTheme = (int)req.ThemeId;
            DatabaseHelper.Instance!.UpdateInstance(connection.Player!.Data);

            connection.SendPacket(new PacketSelectPhoneThemeScRsp(req.ThemeId));
        }
    }
}
