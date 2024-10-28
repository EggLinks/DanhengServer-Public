using EggLink.DanhengServer.Database.Account;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("account", "Game.Command.Account.Desc", "Game.Command.Account.Usage", permission: "egglink.manage")]
public class CommandAccount : ICommand
{
    [CommandMethod("create")]
    public async ValueTask CreateAccount(CommandArg arg)
    {
        if (arg.Args.Count < 2)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var account = arg.Args[1];
        var uid = 0;

        if (arg.Args.Count > 2)
            if (!int.TryParse(arg.Args[2], out uid))
            {
                await arg.SendMsg(I18NManager.Translate("Game.Command.Account.InvalidUid"));
                return;
            }

        if (AccountData.GetAccountByUserName(account) != null)
        {
            await arg.SendMsg(string.Format(I18NManager.Translate("Game.Command.Account.DuplicateAccount"), account));
            return;
        }

        if (uid != 0 && AccountData.GetAccountByUid(uid) != null)
        {
            await arg.SendMsg(string.Format(I18NManager.Translate("Game.Command.Account.DuplicateUID"), uid));
            return;
        }

        try
        {
            AccountHelper.CreateAccount(account, uid);
            await arg.SendMsg(I18NManager.Translate("Game.Command.Account.CreateSuccess", account));
        }
        catch (Exception ex)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Account.CreateError", ex.Message));
        }
    }
}