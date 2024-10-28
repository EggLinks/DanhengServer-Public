using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("setlevel", "Game.Command.Setlevel.Desc", "Game.Command.Setlevel.Usage", ["level"])]
public class CommandSetlevel : ICommand
{
    [CommandDefault]
    public async ValueTask SetLevel(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        if (arg.Args.Count < 1)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var player = arg.Target.Player!;
        var level = Math.Max(Math.Min(arg.GetInt(0), 70), 1);
        player.Data.Level = level;
        player.OnLevelChange();
        player.Data.Exp = GameData.GetPlayerExpRequired(level);
        await player.SendPacket(new PacketPlayerSyncScNotify(player.ToProto()));

        await arg.SendMsg(I18NManager.Translate("Game.Command.Setlevel.SetlevelSuccess"));
    }
}