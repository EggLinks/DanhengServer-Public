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
    [Opcode(CmdIds.SetGameplayBirthdayCsReq)]
    public class HandlerSetGameplayBirthdayCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SetGameplayBirthdayCsReq.Parser.ParseFrom(data);
            var month = req.Birthday / 100;
            var day = req.Birthday % 100;
            if (month < 1 || month > 12 || day < 1 || day > 31)
            {
                connection.SendPacket(new PacketSetGameplayBirthdayScRsp());
                return;
            }
            var playerData = connection.Player!.Data;
            if (playerData.Birthday != 0)
            {
                connection.SendPacket(new PacketSetGameplayBirthdayScRsp());
                return;
            }
            playerData.Birthday = (int)req.Birthday;
            DatabaseHelper.Instance!.UpdateInstance(playerData);
            connection.SendPacket(new PacketSetGameplayBirthdayScRsp(req.Birthday));
        }
    }
}
