using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EggLink.DanhengServer.Database.Account;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("account", "Game.Command.Account.Desc", "Game.Command.Account.Usage", permission: "egglink.manage")]
    public class CommandAccount : ICommand
    {
        [CommandMethod("create")]
        public void CreateAccount(CommandArg arg)
        {
            if (arg.Args.Count < 2)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            string account = arg.Args[1];
            int uid = 0;

            if (arg.Args.Count > 2)
            {
                if (!int.TryParse(arg.Args[2], out uid))
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Account.InvalidUid"));
                    return;
                }
            }

            if (AccountData.GetAccountByUserName(account) != null)
            {
                arg.SendMsg(string.Format(I18nManager.Translate("Game.Command.Account.DuplicateAccount"), account));
                return;
            }

            if (uid != 0 && AccountData.GetAccountByUid(uid) != null)
            {
                arg.SendMsg(string.Format(I18nManager.Translate("Game.Command.Account.DuplicateUID"), uid));
                return;
            }

            try
            {
                AccountHelper.CreateAccount(account, uid);
                arg.SendMsg(I18nManager.Translate("Game.Command.Account.CreateSuccess", account));
            }
            catch (Exception ex)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Account.CreateError", ex.Message));
                return;
            }
        }
    }
}
