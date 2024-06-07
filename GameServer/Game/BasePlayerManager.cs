using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game
{
    public class BasePlayerManager(PlayerInstance player)
    {
        public PlayerInstance Player { get; private set; } = player;
    }
}
