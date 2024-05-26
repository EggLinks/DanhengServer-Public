using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Activity
{
    public class ActivityManager(PlayerInstance player) : BasePlayerManager(player)
    {
        public List<ActivityScheduleData> ToProto()
        {
            var proto = new List<ActivityScheduleData>();

            foreach (var activity in GameData.ActivityConfig.ScheduleData)
            {
                proto.Add(new ActivityScheduleData()
                {
                    ActivityId = (uint)activity.ActivityId,
                    BeginTime = activity.BeginTime,
                    EndTime = activity.EndTime,
                    PanelId = (uint)activity.PanelId,
                });
            }

            return proto;
        }
    }
}
