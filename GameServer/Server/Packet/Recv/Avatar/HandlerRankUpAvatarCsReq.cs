using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.RankUpAvatarCsReq)]
    public class HandlerRankUpAvatarCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = RankUpAvatarCsReq.Parser.ParseFrom(data);
            connection.Player!.InventoryManager?.RankUpAvatar((int)req.BaseAvatarId, req.CostData);
            connection.SendPacket(CmdIds.RankUpAvatarScRsp);
        }
    }
}
