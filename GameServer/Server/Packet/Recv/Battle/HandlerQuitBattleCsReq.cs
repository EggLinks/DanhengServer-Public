using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.QuitBattleCsReq)]
    public class HandlerQuitBattleCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.Player!.BattleInstance = null;
            connection.SendPacket(CmdIds.QuitBattleScRsp);
        }
    }
}
