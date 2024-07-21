using EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;
using EggLink.DanhengServer.Server;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.HeartDial
{
    [Opcode(CmdIds.GetHeartDialInfoCsReq)]
    public class HandlerGetHeartDialInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(new PacketGetHeartDialInfoScRsp(connection.Player!));
        }
    }
}
