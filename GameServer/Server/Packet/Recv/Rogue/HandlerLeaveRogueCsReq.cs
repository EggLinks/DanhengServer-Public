using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Rogue
{
    [Opcode(CmdIds.LeaveRogueCsReq)]
    public class HandlerLeaveRogueCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            if (player.RogueManager?.RogueInstance != null)
            {
                player.RogueManager.RogueInstance.LeaveRogue();
            }
            connection.SendPacket(new PacketLeaveRogueScRsp(player));
        }
    }
}
