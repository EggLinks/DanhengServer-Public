using EggLink.DanhengServer.Server.Packet.Send.Gacha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Gacha
{
    [Opcode(CmdIds.GetGachaInfoCsReq)]
    public class HandlerGetGachaInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetGachaInfoScRsp(connection.Player!));
        }
    }
}
