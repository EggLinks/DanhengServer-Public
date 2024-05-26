using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("unlockall", "Game.Command.UnlockAll.Desc", "Game.Command.UnlockAll.Usage")]
    public class CommandUnlockAll : ICommand
    {
        [CommandMethod("0 mission")]
        public void UnlockAllMissions(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var player = arg.Target!.Player!;
            var missionManager = player.MissionManager!;

            foreach (var mission in GameData.SubMissionData.Values)
            {
                missionManager.Data.SetSubMissionStatus(mission.SubMissionID, MissionPhaseEnum.Finish);
            }

            foreach (var mission in GameData.MainMissionData.Values)
            {
                missionManager.Data.SetMainMissionStatus(mission.MainMissionID, MissionPhaseEnum.Finish);
            }

            if (player.Data.CurrentGender == Gender.Man)
            {
                player.Data.CurrentGender = Gender.Man;
                player.Data.CurBasicType = 8001;
            } else
            {
                player.Data.CurrentGender = Gender.Woman;
                player.Data.CurBasicType = 8002;
                player.AvatarManager!.GetHero()!.HeroId = 8002;
            }

            arg.SendMsg(I18nManager.Translate("Game.Command.UnlockAll.AllMissionsUnlocked"));
            arg.Target!.Player!.SendPacket(new PacketPlayerKickOutScNotify());
            arg.Target!.Stop();
        }
    }
}
