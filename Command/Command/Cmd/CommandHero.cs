using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("hero", "Game.Command.Hero.Desc", "Game.Command.Hero.Usage")]
public class CommandHero : ICommand
{
    [CommandMethod("0 gender")]
    public async ValueTask ChangeGender(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        if (arg.BasicArgs.Count < 1)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var gender = (Gender)arg.GetInt(0);
        if (gender == Gender.None)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Hero.GenderNotSpecified"));
            return;
        }

        var player = arg.Target!.Player!;
        player.Data.CurrentGender = gender;
        await player.ChangeAvatarPathType(8001, MultiPathAvatarTypeEnum.Warrior);
        await player.SendPacket(new PacketGetMultiPathAvatarInfoScRsp(player));

        await arg.SendMsg(I18NManager.Translate("Game.Command.Hero.GenderChanged"));
    }

    [CommandMethod("0 type")]
    public async ValueTask ChangeType(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        if (arg.BasicArgs.Count < 1)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var gender = (MultiPathAvatarTypeEnum)arg.GetInt(0);
        if (gender == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Hero.HeroTypeNotSpecified"));
            return;
        }

        var player = arg.Target!.Player!;
        await player.ChangeAvatarPathType(8001, gender);

        await arg.SendMsg(I18NManager.Translate("Game.Command.Hero.HeroTypeChanged"));
    }
}