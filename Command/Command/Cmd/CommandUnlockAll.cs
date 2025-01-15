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

        await arg.SendMsg(I18NManager.Translate("Game.Command.UnlockAll.UnlockedAll",
            I18NManager.Translate("Word.Mission")));
        await arg.Target!.Player!.SendPacket(new PacketPlayerKickOutScNotify());
        arg.Target!.Stop();
    }

    [CommandMethod("0 tutorial")]
    public async ValueTask UnlockAllTutorial(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target!.Player!;

        foreach (var data in GameData.TutorialDataData)
            player.TutorialData!.Tutorials[data.Key] = TutorialStatus.TutorialFinish;

        foreach (var data in GameData.TutorialGuideDataData)
            player.TutorialGuideData!.Tutorials[data.Key] = TutorialStatus.TutorialFinish;

        await arg.SendMsg(I18NManager.Translate("Game.Command.UnlockAll.UnlockedAll",
            I18NManager.Translate("Word.Tutorial")));
        await arg.Target!.Player!.SendPacket(new PacketPlayerKickOutScNotify());
        arg.Target!.Stop();
    }

    [CommandMethod("0 rogue")]
    public async ValueTask UnlockAllRogue(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target!.Player!;
        List<int> swarmList =
            [8013101, 8013102, 8013103, 8013104, 8013105, 8013106, 8013107, 8013108, 8013109, 8013110];
        List<int> ggList = [8016101, 8016102, 8016103, 8016104, 8016105, 8016106];
        List<int> duList = [8023401, 8023501, 8023601];
        List<int> udList = [8026401, 8026402];

        List<int> allList = [.. swarmList, .. ggList, .. duList, .. udList];

        foreach (var id in allList)
        {
            // finish mission
            await player.MissionManager!.AcceptMainMission(id);
            await player.MissionManager!.FinishMainMission(id);
        }

        await arg.SendMsg(I18NManager.Translate("Game.Command.UnlockAll.UnlockedAll",
            I18NManager.Translate("Word.TypesOfRogue")));
        await arg.Target!.Player!.SendPacket(new PacketPlayerKickOutScNotify());
        arg.Target!.Stop();
    }
}