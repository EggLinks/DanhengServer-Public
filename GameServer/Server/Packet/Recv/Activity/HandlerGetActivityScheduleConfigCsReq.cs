using EggLink.DanhengServer.Server.Packet.Send.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Activity
{
    [Opcode(CmdIds.GetActivityScheduleConfigCsReq)]
    public class HandlerGetActivityScheduleConfigCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetActivityScheduleConfigScRsp(connection.Player!));
        }
    }
}
