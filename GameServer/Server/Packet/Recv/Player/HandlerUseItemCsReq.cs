using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.UseItemCsReq)]
    public class HandlerUseItemCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = UseItemCsReq.Parser.ParseFrom(data);

            connection.SendPacket(CmdIds.UseItemScRsp);
        }
    }
}
