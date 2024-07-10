using EggLink.DanhengServer.Database.Lineup;
using EggLink.DanhengServer.Util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Database.Mission
{
    [SugarTable("StoryLineData")]
    public class StoryLineData : BaseDatabaseDataHelper
    {
        public int CurStoryLineId { get; set; }

        public int OldPlaneId { get; set; }
        public int OldFloorId { get; set; }
        public int OldEntryId { get; set; }

        [SugarColumn(IsJson = true)]
        public Position OldPos { get; set; } = new();

        [SugarColumn(IsJson = true)]
        public Position OldRot { get; set; } = new();

        [SugarColumn(IsJson = true)]
        public Dictionary<int, StoryLineInfo> RunningStoryLines { get; set; } = [];  // finished one will be deleted
    }

    public class StoryLineInfo
    {
        public int StoryLineId { get; set; }

        // Save Data
        public int SavedPlaneId { get; set; }
        public int SavedFloorId { get; set; }
        public int SavedEntryId { get; set; }
        public Position SavedPos { get; set; } = new();
        public Position SavedRot { get; set; } = new();
        public List<LineupAvatarInfo> Lineup { get; set; } = [];
    }
}
