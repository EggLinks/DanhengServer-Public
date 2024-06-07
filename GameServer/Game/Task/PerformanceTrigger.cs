using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Task
{
    public class PerformanceTrigger(PlayerInstance player) : BasePlayerManager(player)
    {
        public void TriggerPerformance(int performanceId)
        {
            GameData.PerformanceEData.TryGetValue(performanceId, out var excel);
            if (excel != null)
            {
                TriggerPerformance(excel);
            }
        }

        public void TriggerPerformance(PerformanceEExcel excel)
        {
            if (excel.ActInfo == null) return;
            foreach (var act in excel.ActInfo.OnInitSequece)
            {
                TriggerAct(act);
            }

            foreach (var act in excel.ActInfo.OnStartSequece)
            {
                TriggerAct(act);
            }
        }

        private void TriggerAct(MissionActTaskInfo act)
        {
            foreach (var task in act.TaskList)
            {
                TriggerTask(task);
            }

            foreach (var task in act.TaskList)
            {
                TriggerTask(task);
            }
        }

        private void TriggerTask(MissionActTaskInfo act)
        {
            try
            {
                var methodName = act.Type.Replace("RPG.GameCore.", "");

                var method = GetType().GetMethod(methodName);
                if (method != null)
                {
                    _ = method.Invoke(this, [act]);
                }
            } catch
            {
            }
        }

        #region Task

        public void PlayMessage(MissionActTaskInfo act)
        {
            Player.MessageManager!.AddMessageSection(act.MessageSectionID);
        }

        #endregion
    }
}
