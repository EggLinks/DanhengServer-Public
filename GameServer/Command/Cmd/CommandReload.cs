using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Custom;
using EggLink.DanhengServer.Internationalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("reload", "Game.Command.Reload.Desc", "Game.Command.Reload.Usage", permission: "egglink.manage")]
    public class CommandReload : ICommand
    {
        [CommandMethod("0 banner")]
        public void ReloadBanner(CommandArg arg)
        {
            // Reload the banners
            GameData.BannersConfig = ResourceManager.LoadCustomFile<BannersConfig>("Banner", "Banners") ?? new();
            arg.SendMsg(I18nManager.Translate("Game.Command.Reload.ConfigReloaded", I18nManager.Translate("Word.Banner")));
        }

        [CommandMethod("0 activity")]
        public void ReloadActivity(CommandArg arg)
        {
            // Reload the activities
            GameData.ActivityConfig = ResourceManager.LoadCustomFile<ActivityConfig>("Activity", "ActivityConfig") ?? new();
            arg.SendMsg(I18nManager.Translate("Game.Command.Reload.ConfigReloaded", I18nManager.Translate("Word.Activity")));
        }
    }
}
