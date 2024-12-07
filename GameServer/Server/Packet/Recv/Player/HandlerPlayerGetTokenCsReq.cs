using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.PlayerGetTokenCsReq)]
public class HandlerPlayerGetTokenCsReq : Handler
{
    private readonly Logger logger = new("GameServer");

    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = PlayerGetTokenCsReq.Parser.ParseFrom(data);

        var account = DatabaseHelper.Instance?.GetInstance<AccountData>(int.Parse(req.AccountUid));
        if (account == null)
        {
            await connection.SendPacket(new PacketPlayerGetTokenScRsp(0, Retcode.RetNotInWhiteList));
            return;
        }

        if (!ResourceManager.IsLoaded)
            // resource manager not loaded, return
            return;

        var prev = Listener.GetActiveConnection(account.Uid);
        if (prev != null)
        {
            await prev.SendPacket(new PacketPlayerKickOutScNotify());
            prev.Stop();
        }

        connection.State = SessionStateEnum.WAITING_FOR_LOGIN;

        var pd = DatabaseHelper.Instance?.GetInstance<PlayerData>(int.Parse(req.AccountUid));
        connection.Player = pd == null ? new PlayerInstance(int.Parse(req.AccountUid)) : new PlayerInstance(pd);

        connection.DebugFile = Path.Combine(ConfigManager.Config.Path.LogPath, "Debug/", $"{req.AccountUid}/",
            $"Debug-{DateTime.Now:yyyy-MM-dd HH-mm-ss}.log");

        await connection.Player.OnGetToken();
        connection.Player.Connection = connection;
        await connection.SendPacket(new PacketPlayerGetTokenScRsp(connection));

        if (ConfigManager.Config.GameServer.UsePacketEncryption)
        {
            connection.XorKey = Crypto.GenerateXorKey(connection.ClientSecretKeySeed);
            logger.Info($"{connection.RemoteEndPoint} key exchange successful");
        }
    }
}