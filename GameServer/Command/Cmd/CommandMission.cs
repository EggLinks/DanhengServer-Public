using EggLink.DanhengServer.Internationalization;
using System.Collections.Generic;
using System.Text;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("mission", "Game.Command.Mission.Desc", "Game.Command.Mission.Usage")]
    public class CommandMission : ICommand
    {
        [CommandMethod("0 pass")]
        public void PassRunningMission(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var mission = arg.Target!.Player!.MissionManager!;
            var count = mission.GetRunningSubMissionIdList().Count;
            mission.GetRunningSubMissionIdList().ForEach(mission.FinishSubMission);
            arg.SendMsg(I18nManager.Translate("Game.Command.Mission.AllRunningMissionsFinished", count.ToString()));
        }

        [CommandMethod("0 finish")]
        public void FinishRunningMission(CommandArg arg)
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

            if (!int.TryParse(arg.BasicArgs[0], out var missionId))
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Mission.InvalidMissionId"));
                return;
            }

            var mission = arg.Target!.Player!.MissionManager!;
            mission.FinishSubMission(missionId);
            arg.SendMsg(I18nManager.Translate("Game.Command.Mission.MissionFinished", missionId.ToString()));
        }

        [CommandMethod("0 running")]
        public void ListRunningMission(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            var mission = arg.Target!.Player!.MissionManager!;
            var runningMissions = mission.GetRunningSubMissionList();
            if (runningMissions.Count == 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Mission.NoRunningMissions"));
                return;
            }

            arg.SendMsg(I18nManager.Translate("Game.Command.Mission.RunningMissions"));
            Dictionary<int, List<int>> missionMap = [];

            foreach (var m in runningMissions)
            {
                if (!missionMap.TryGetValue(m.MainMissionID, out List<int>? value))
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
                arg.SendMsg($"{I18nManager.Translate("Game.Command.Mission.MainMission")} {list.Key}：");
                var sb = new StringBuilder();
                foreach (var id in list.Value)
                {
                    sb.Append($"{id}、");

                    if (id.ToString().StartsWith("10"))
                    {
                        possibleStuckIds.Add(id);

                        var info = mission.GetSubMissionInfo(id);
                        if (info != null && info.FinishType == Enums.MissionFinishTypeEnum.PropState)
                        {
                            morePossibleStuckIds.Add(id);
                        }
                    }
                }

                sb.Remove(sb.Length - 1, 1);

                arg.SendMsg(sb.ToString());
            }

            if (morePossibleStuckIds.Count > 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Mission.PossibleStuckMissions"));

                var sb = new StringBuilder();
                foreach (var id in morePossibleStuckIds)
                {
                    sb.Append($"{id}、");
                }

                sb.Remove(sb.Length - 1, 1);

                arg.SendMsg(sb.ToString());
            }
            else if (possibleStuckIds.Count > 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Mission.PossibleStuckMissions"));

                var sb = new StringBuilder();
                foreach (var id in possibleStuckIds)
                {
                    sb.Append($"{id}、");
                }

                sb.Remove(sb.Length - 1, 1);

                arg.SendMsg(sb.ToString());
            }
        }

        [CommandMethod("0 reaccept")]
        public void ReAcceptMission(CommandArg arg)
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

            if (!int.TryParse(arg.BasicArgs[0], out var missionId))
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Mission.InvalidMissionId"));
                return;
            }

            var mission = arg.Target!.Player!.MissionManager!;
            mission.ReAcceptMainMission(missionId);
            arg.SendMsg(I18nManager.Translate("Game.Command.Mission.MissionReAccepted", missionId.ToString()));
        }

        [CommandMethod("0 finishmain")]
        public void FinishMainMission(CommandArg arg)
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

            if (!int.TryParse(arg.BasicArgs[0], out var missionId))
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Mission.InvalidMissionId"));
                return;
            }

            var mission = arg.Target!.Player!.MissionManager!;
            mission.FinishMainMission(missionId);
            arg.SendMsg(I18nManager.Translate("Game.Command.Mission.MissionFinished", missionId.ToString()));
        }
    }
}
