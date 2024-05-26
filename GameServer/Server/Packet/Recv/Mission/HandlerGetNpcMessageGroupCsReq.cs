using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.GetNpcMessageGroupCsReq)]
    public class HandlerGetNpcMessageGroupCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetNpcMessageGroupCsReq.Parser.ParseFrom(data);

            connection.SendPacket(new PacketGetNpcMessageGroupScRsp(req.ContactIdList, connection.Player!));
        }
    }
}
