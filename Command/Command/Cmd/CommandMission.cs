using System.Text;
using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("mission", "Game.Command.Mission.Desc", "Game.Command.Mission.Usage", ["m"])]
public class CommandMission : ICommand
{
    [CommandMethod("0 pass")]
    public async ValueTask PassRunningMission(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var mission = arg.Target!.Player!.MissionManager!;
        var count = mission.GetRunningSubMissionIdList().Count;
        foreach (var id in mission.GetRunningSubMissionIdList()) await mission.FinishSubMission(id);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.AllRunningMissionsFinished", count.ToString()));
    }

    [CommandMethod("0 finish")]
    public async ValueTask FinishRunningMission(CommandArg arg)
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

        if (!int.TryParse(arg.BasicArgs[0], out var missionId))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.InvalidMissionId"));
            return;
        }

        var mission = arg.Target!.Player!.MissionManager!;
        await mission.FinishSubMission(missionId);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.MissionFinished", missionId.ToString()));
    }

    [CommandMethod("0 running")]
    public async ValueTask ListRunningMission(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var mission = arg.Target!.Player!.MissionManager!;
        var runningMissions = mission.GetRunningSubMissionList();
        if (runningMissions.Count == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.NoRunningMissions"));
            return;
        }

        await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.RunningMissions"));
        Dictionary<int, List<int>> missionMap = [];

        foreach (var m in runningMissions)
        {
            if (!missionMap.TryGetValue(m.MainMissionID, out var value))
            {
                value = [];
                missionMap[m.MainMissionID] = value;
            }

            value.Add(m.ID);
        }

        var possibleStuckIds = new List<int>();
        var morePossibleStuckIds = new List<int>();

        foreach (var list in missionMap)
        {
            await arg.SendMsg($"{I18NManager.Translate("Game.Command.Mission.MainMission")} {list.Key}：");
            var sb = new StringBuilder();
            foreach (var id in list.Value)
            {
                sb.Append($"{id}、");

                if (!id.ToString().StartsWith("10")) continue;
                possibleStuckIds.Add(id);

                var info = mission.GetSubMissionInfo(id);
                if (info?.FinishType == MissionFinishTypeEnum.PropState) morePossibleStuckIds.Add(id);
            }

            sb.Remove(sb.Length - 1, 1);

            await arg.SendMsg(sb.ToString());
        }

        if (morePossibleStuckIds.Count > 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.PossibleStuckMissions"));

            var sb = new StringBuilder();
            foreach (var id in morePossibleStuckIds) sb.Append($"{id}、");

            sb.Remove(sb.Length - 1, 1);

            await arg.SendMsg(sb.ToString());
        }
        else if (possibleStuckIds.Count > 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.PossibleStuckMissions"));

            var sb = new StringBuilder();
            foreach (var id in possibleStuckIds) sb.Append($"{id}、");

            sb.Remove(sb.Length - 1, 1);

            await arg.SendMsg(sb.ToString());
        }

        await Task.CompletedTask;
    }

    [CommandMethod("0 reaccept")]
    public async ValueTask ReAcceptMission(CommandArg arg)
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

        if (!int.TryParse(arg.BasicArgs[0], out var missionId))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.InvalidMissionId"));
            return;
        }

        var mission = arg.Target!.Player!.MissionManager!;
        await mission.ReAcceptMainMission(missionId);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.MissionReAccepted", missionId.ToString()));
    }

    [CommandMethod("0 finishmain")]
    public async ValueTask FinishMainMission(CommandArg arg)
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

        if (!int.TryParse(arg.BasicArgs[0], out var missionId))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.InvalidMissionId"));
            return;
        }

        var mission = arg.Target!.Player!.MissionManager!;
        await mission.FinishMainMission(missionId);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Mission.MissionFinished", missionId.ToString()));
    }
}