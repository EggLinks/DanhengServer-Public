using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event
{
    public abstract class RogueEventCostHandler
    {
        public abstract void Handle(BaseRogueInstance rogue, RogueEventInstance? eventInstance, List<int> ParamList);
    }
}
