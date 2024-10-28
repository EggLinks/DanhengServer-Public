using EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("lineup", "Game.Command.Lineup.Desc", "Game.Command.Lineup.Usage")]
public class CommandLineup : ICommand
{
    [CommandMethod("0 mp")]
    public async ValueTask SetLineupMp(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var count = arg.GetInt(0);
        await arg.Target.Player!.LineupManager!.GainMp(count == 0 ? 2 : count);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Lineup.PlayerGainedMp", count.ToString()));
    }

    [CommandMethod("0 heal")]
    public async ValueTask HealLineup(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target.Player!;
        foreach (var avatar in player.LineupManager!.GetCurLineup()!.AvatarData!.Avatars) avatar.CurrentHp = 10000;
        await player.SendPacket(new PacketSyncLineupNotify(player.LineupManager.GetCurLineup()!));
        await arg.SendMsg(I18NManager.Translate("Game.Command.Lineup.HealedAllAvatars"));
    }
}