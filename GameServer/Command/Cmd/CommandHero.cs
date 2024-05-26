using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("hero", "Game.Command.Hero.Desc", "Game.Command.Hero.Usage")]
    public class CommandHero : ICommand
    {
        [CommandMethod("0 gender")]
        public void ChangeGender(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            if (arg.BasicArgs.Count < 1)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            var gender = (Gender)arg.GetInt(0);
            if (gender == Gender.None)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Hero.GenderNotSpecified"));
                return;
            }

            var player = arg.Target!.Player!;
            player.Data.CurrentGender = gender;
            player.ChangeHeroBasicType(HeroBasicTypeEnum.Warrior);
            player.SendPacket(new PacketGetHeroBasicTypeInfoScRsp(player));

            arg.SendMsg(I18nManager.Translate("Game.Command.Hero.GenderChanged"));
        }

        [CommandMethod("0 type")]
        public void ChangeType(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            if (arg.BasicArgs.Count < 1)
            {
                arg.SendMsg("");
                return;
            }

            var gender = (HeroBasicTypeEnum)arg.GetInt(0);
            if (gender == 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Hero.HeroTypeNotSpecified"));
                return;
            }

            var player = arg.Target!.Player!;
            player.ChangeHeroBasicType(gender);

            arg.SendMsg(I18nManager.Translate("Game.Command.Hero.HeroTypeChanged"));
        }
    }
}
