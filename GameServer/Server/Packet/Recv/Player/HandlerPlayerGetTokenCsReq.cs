using EggLink.DanhengServer.Common.Enums;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.PlayerGetTokenCsReq)]
    public class HandlerPlayerGetTokenCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = PlayerGetTokenCsReq.Parser.ParseFrom(data);

            var account = DatabaseHelper.Instance?.GetInstance<AccountData>(int.Parse(req.AccountUid));
            if (account == null)
            {
                connection.SendPacket(new PacketPlayerGetTokenScRsp());
                connection.SendPacket(new PacketPlayerKickOutScNotify());
                return;
            }

            var prev = Listener.GetActiveConnection(account.Uid);
            if (prev != null)
            {
                prev.SendPacket(new PacketPlayerKickOutScNotify());
                prev.Stop();
            }

            connection.State = SessionState.WAITING_FOR_LOGIN;
            var pd = DatabaseHelper.Instance?.GetInstance<PlayerData>(int.Parse(req.AccountUid));
            if (pd == null)
                connection.Player = new PlayerInstance(int.Parse(req.AccountUid));
            else
                connection.Player = new PlayerInstance(pd);
            connection.Player.OnLogin();
            connection.Player.Connection = connection;
            connection.SendPacket(new PacketPlayerGetTokenScRsp(connection));
        }
    }
}
