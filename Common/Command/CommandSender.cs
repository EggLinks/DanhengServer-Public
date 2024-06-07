using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Account;
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

        public int GetSender();
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
        public int GetSender()
        {
            return 0;
        }
    }
}
