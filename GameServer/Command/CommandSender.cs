using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command
{
    public interface ICommandSender
    {
        public void SendMsg(string msg);

        public bool HasPermission(string permission);
    }

    public class ConsoleCommandSender(Logger logger) : ICommandSender
    {
        public void SendMsg(string msg)
        {
            logger.Info(msg);
        }

        public bool HasPermission(string permission)
        {
            return true;
        }
    }

    public class PlayerCommandSender(PlayerInstance player) : ICommandSender
    {
        public PlayerInstance Player = player;

        public void SendMsg(string msg)
        {
            Player.SendPacket(new PacketRevcMsgScNotify(toUid:Player.Uid, fromUid: (uint)ConfigManager.Config.ServerOption.ServerProfile.Uid, msg));
        }

        public bool HasPermission(string permission)
        {
            var account = DatabaseHelper.Instance!.GetInstance<AccountData>(Player.Uid)!;
            return account.Permissions!.Contains(permission);
        }
    }
}
