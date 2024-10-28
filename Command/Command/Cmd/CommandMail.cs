using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("mail", "Game.Command.Mail.Desc", "Game.Command.Mail.Usage", permission: "egglink.manage")]
public class CommandMail : ICommand
{
    [CommandDefault]
    public async ValueTask Mail(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        if (arg.Args.Count < 7)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        if (!(arg.Args.Contains("_TITLE") && arg.Args.Contains("_CONTENT")))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var sender = arg.Args[0];
        var templateId = int.Parse(arg.Args[1]);
        var expiredDay = int.Parse(arg.Args[2]);

        var title = "";
        var content = "";

        var flagTitle = false;
        var flagContent = false;
        foreach (var text in arg.Args)
        {
            switch (text)
            {
                case "_TITLE":
                    flagTitle = true;
                    continue;
                case "_CONTENT":
                    flagContent = true;
                    continue;
            }

            if (flagTitle && !flagContent) title += text + " ";


            if (flagTitle && flagContent) content += text + " ";
        }

        content = content[..^1];
        title = title[..^1];

        await arg.Target.Player!.MailManager!.SendMail(sender, title, content, templateId, expiredDay);

        await arg.SendMsg(I18NManager.Translate("Game.Command.Mail.MailSent"));
    }
}