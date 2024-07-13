using EggLink.DanhengServer.Internationalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Command.Cmd
{
    [CommandInfo("raid", "Game.Command.Raid.Desc", "Game.Command.Raid.Usage", permission: "")]
    public class CommandRaid : ICommand
    {
        [CommandMethod("0 leave")]
        public void Leave(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            arg.Target.Player!.RaidManager!.LeaveRaid(false);

            arg.SendMsg(I18nManager.Translate("Game.Command.Raid.Leaved"));
        }
    }
}
