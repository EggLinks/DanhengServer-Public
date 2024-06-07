using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.DeployRotaterCsReq)]
    public class HandlerDeployRotaterCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = DeployRotaterCsReq.Parser.ParseFrom(data);

            connection.Player!.ChargerNum--;
            connection.SendPacket(new PacketDeployRotaterScRsp(req.RotaterData, connection.Player!.ChargerNum, 5));
        }
    }
}
