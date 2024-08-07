using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Chat;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Command;

public class PlayerCommandSender(PlayerInstance player) : ICommandSender
{
    public PlayerInstance Player = player;

    public async ValueTask SendMsg(string msg)
    {
        await Player.SendPacket(new PacketRevcMsgScNotify((uint)Player.Uid,
            (uint)ConfigManager.Config.ServerOption.ServerProfile.Uid, msg.Replace("\n", "    ")));
    }

    public bool HasPermission(string permission)
    {
        var account = DatabaseHelper.Instance!.GetInstance<AccountData>(Player.Uid)!;
        return account.Permissions!.Contains(permission);
    }

    public int GetSender()
    {
        return Player.Uid;
    }
}