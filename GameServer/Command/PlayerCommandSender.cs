using EggLink.DanhengServer.Command;
using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Command
{
    public class PlayerCommandSender(PlayerInstance player) : ICommandSender
    {
        public PlayerInstance Player = player;

        public void SendMsg(string msg)
        {
            Player.SendPacket(new PacketRevcMsgScNotify(toUid: (uint)Player.Uid, fromUid: (uint)ConfigManager.Config.ServerOption.ServerProfile.Uid, msg));
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
}
