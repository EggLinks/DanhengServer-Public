using EggLink.DanhengServer.Internationalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("lineup", "Game.Command.Lineup.Desc", "Game.Command.Lineup.Usage")]
    public class CommandLineup : ICommand
    {
        [CommandMethod("0 mp")]
        public void SetLineupMp(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var count = arg.GetInt(0);
            arg.Target.Player!.LineupManager!.GainMp(count == 0 ? 2: count);
            arg.SendMsg(I18nManager.Translate("Game.Command.Lineup.PlayerGainedMp", Math.Min(count, 2).ToString()));
        }

        [CommandMethod("0 heal")]
        public void HealLineup(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            var player = arg.Target.Player!;
            foreach (var avatar in player.LineupManager!.GetCurLineup()!.AvatarData!.Avatars)
            {
                avatar.CurrentHp = 10000;
            }
            player.SceneInstance!.SyncLineup();
            arg.SendMsg(I18nManager.Translate("Game.Command.Lineup.HealedAllAvatars"));
        }
    }
}
