using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("unstuck", "Game.Command.Unstuck.Desc", "Game.Command.Unstuck.Usage")]
public class CommandUnstuck : ICommand
{
    [CommandDefault]
    public async ValueTask Unstuck(CommandArg arg)
    {
        if (arg.Target != null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Unstuck.PlayerIsOnline"));
            return;
        }

        if (arg.BasicArgs.Count == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var playerData = InitializeDatabase<PlayerData>(arg.GetInt(0));

        if (playerData != null)
        {
            playerData.Pos = new Position(99, 62, -4800);
            playerData.Rot = new Position();
            playerData.PlaneId = 20001;
            playerData.FloorId = 20001001;
            playerData.EntryId = 2000101;
            await arg.SendMsg(I18NManager.Translate("Game.Command.Unstuck.UnstuckSuccess"));
        }
        else
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Unstuck.UidNotExist"));
        }
    }

    public T? InitializeDatabase<T>(int uid) where T : class, new()
    {
        var instance = DatabaseHelper.Instance?.GetInstance<T>(uid);
        return instance;
    }
}