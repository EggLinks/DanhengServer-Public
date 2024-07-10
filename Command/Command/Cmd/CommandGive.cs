using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Others;
using EggLink.DanhengServer.Server.Packet.Send.Tutorial;
using Org.BouncyCastle.Ocsp;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("give", "Give item to player", "give <item> l<level> x<amount> r<rank>")]
    public class CommandGive : ICommand
    {
        [CommandDefault]
        public void GiveItem(CommandArg arg)
        {
            var player = arg.Target?.Player;
            if (player == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            if (arg.BasicArgs.Count == 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            GameData.ItemConfigData.TryGetValue(arg.GetInt(0), out var itemData);
            if (itemData == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Give.ItemNotFound"));
                return;
            }

            arg.CharacterArgs.TryGetValue("x", out var str);
            arg.CharacterArgs.TryGetValue("l", out var levelStr);
            arg.CharacterArgs.TryGetValue("r", out var rankStr);
            str ??= "1";
            levelStr ??= "1";
            rankStr ??= "1";
            if (!int.TryParse(str, out var amount) || !int.TryParse(levelStr, out var level) || !int.TryParse(rankStr, out var rank))
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            player.InventoryManager!.AddItem(arg.GetInt(0), amount, rank: Math.Min(rank, 5), level: Math.Max(Math.Min(level, 80), 1));

            arg.SendMsg(I18nManager.Translate("Game.Command.Give.GiveItem", player.Uid.ToString(), amount.ToString(), itemData.Name ?? itemData.ID.ToString()));
        }
    }
}
