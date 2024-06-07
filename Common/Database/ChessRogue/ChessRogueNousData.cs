using EggLink.DanhengServer.Proto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Database.ChessRogue
{
    [SugarTable("ChessRogueNous")]
    public class ChessRogueNousData : BaseDatabaseDataHelper
    {
        [SugarColumn(IsJson = true)]
        public Dictionary<int, ChessRogueNousDiceData> RogueDiceData { get; set; } = [];
    }

    public class ChessRogueNousDiceData
    {
        public int BranchId { get; set; }
        public Dictionary<int, int> Surfaces { get; set; } = [];
        public int AreaId { get; set; }
        public int DifficultyLevel { get; set; }

        public ChessRogueDice ToProto()
        {
            return new ChessRogueDice()
            {
                BranchId = (uint)BranchId,
                SurfaceList = { Surfaces.Select(x => new ChessRogueDiceSurfaceInfo() { Index = (uint)x.Key, SurfaceId = (uint)x.Value }) },
                //AreaId = (uint)AreaId,
                //DifficultyLevel = (uint)DifficultyLevel,
            };
        }
    }
}
