using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.PVEBattleResultCsReq)]
    public class HandlerPVEBattleResultCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = PVEBattleResultCsReq.Parser.ParseFrom(data);
            connection.Player?.BattleManager?.EndBattle(req);
        }
    }
}
