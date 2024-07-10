using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.ContentPackageGetDataCsReq)]
    public class HandlerContentPackageGetDataCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = ContentPackageGetDataCsReq.Parser.ParseFrom(data);

            //connection.SendPacket(new PacketContentPackageGetDataScRsp(req.ContentId));  // cause crash (not only SR but also ur PC(or other program) 
        }
    }
}
