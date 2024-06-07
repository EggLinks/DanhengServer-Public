using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Custom
{
    public class ChessRogueRoomConfig
    {
        public int EntranceId { get; set; }
        public List<int> Groups { get; set; } = [];
        public Dictionary<int, ChessRogueRoom> CellGroup { get; set; } = [];
    }

    public class ChessRogueRoom
    {
        public List<int> Groups { get; set; } = [];
        public bool IsBoss { get; set; } = false;
        public bool IsLastBoss { get; set; } = false;
    }
}
