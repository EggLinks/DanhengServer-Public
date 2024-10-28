using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("unlockall", "Game.Command.UnlockAll.Desc", "Game.Command.UnlockAll.Usage", ["ua"])]
public class CommandUnlockAll : ICommand
{
    [CommandMethod("0 mission")]
    public async ValueTask UnlockAllMissions(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target!.Player!;
        var missionManager = player.MissionManager!;

        foreach (var mission in GameData.SubMissionData.Values)
            missionManager.Data.SetSubMissionStatus(mission.SubMissionID, MissionPhaseEnum.Finish);

        foreach (var mission in GameData.MainMissionData.Values)
            missionManager.Data.SetMainMissionStatus(mission.MainMissionID, MissionPhaseEnum.Finish);

        if (player.Data.CurrentGender == Gender.Man)
        {
            player.Data.CurrentGender = Gender.Man;
            player.Data.CurBasicType = 8001;
        }
        else
        {
            player.Data.CurrentGender = Gender.Woman;
            player.Data.CurBasicType = 8002;
            player.AvatarManager!.GetHero()!.PathId = 8002;
        }

        await arg.SendMsg(I18NManager.Translate("Game.Command.UnlockAll.AllMissionsUnlocked"));
        await arg.Target!.Player!.SendPacket(new PacketPlayerKickOutScNotify());
        arg.Target!.Stop();
    }
}