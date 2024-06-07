using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Excel
{
    [ResourceEntity("MazeBuff.json")]
    public class MazeBuffExcel : ExcelResource
    {
        public int ID { get; set; }
        public int Lv { get; set; }

        public override int GetId()
        {
            return ID * 10 + Lv;
        }

        public override void Loaded()
        {
            GameData.MazeBuffData.Add(GetId(), this);
        }

        public BattleBuff ToProto()
        {
            return new BattleBuff()
            {
                Id = (uint)ID,
                Level = (uint)Lv,
            };
        }
    }
}
