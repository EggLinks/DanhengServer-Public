using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("kick", "Game.Command.Kick.Desc", "Game.Command.Kick.Usage", permission: "egglink.manage")]
    public class CommandKick : ICommand
    {
        [CommandDefault]
        public void Kick(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            arg.Target.SendPacket(new PacketPlayerKickOutScNotify());
            arg.SendMsg(I18nManager.Translate("Game.Command.Kick.PlayerKicked", arg.Target.Player!.Data.Name!));
            arg.Target.Stop();
        }
    }
}
