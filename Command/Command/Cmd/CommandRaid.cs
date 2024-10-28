using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("raid", "Game.Command.Raid.Desc", "Game.Command.Raid.Usage", permission: "")]
public class CommandRaid : ICommand
{
    [CommandMethod("0 leave")]
    public async ValueTask Leave(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        await arg.Target.Player!.RaidManager!.LeaveRaid(false);

        await arg.SendMsg(I18NManager.Translate("Game.Command.Raid.Leaved"));
    }
}