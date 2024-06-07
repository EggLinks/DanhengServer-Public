using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Scene
{
    [Opcode(CmdIds.GetNpcTakenRewardCsReq)]
    public class HandlerGetNpcTakenRewardCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetNpcTakenRewardCsReq.Parser.ParseFrom(data);

            connection.SendPacket(new PacketGetNpcTakenRewardScRsp(req.NpcId));
        }
    }
}
