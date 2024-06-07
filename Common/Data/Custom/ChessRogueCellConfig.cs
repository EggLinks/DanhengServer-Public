using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Data.Custom
{
    public class ChessRogueCellConfig
    {
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public double RotY { get; set; }

        public List<int> Groups { get; set; } = [];

        public Position ToPosition() => new((int) (PosX * 10000), (int) (PosY * 10000), (int) (PosZ * 10000));

        public Position ToRotation() => new(0, (int) RotY * 10000, 0);
    }
}
