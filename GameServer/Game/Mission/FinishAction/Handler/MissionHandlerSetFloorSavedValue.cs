using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Mission.FinishAction.Handler
{
    [MissionFinishAction(FinishActionTypeEnum.SetFloorSavedValue)]
    public class MissionHandlerSetFloorSavedValue : MissionFinishActionHandler
    {
        public override void OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
        {
            _ = int.TryParse(ParamString[1], out var floor);
            Player.SceneData!.FloorSavedData.TryGetValue(floor, out var value);
            if (value == null)
            {
                value = [];
                Player.SceneData.FloorSavedData[floor] = value;
            }

            value[ParamString[2]] = int.Parse(ParamString[3]);  // ParamString[2] is the key
        }
    }
}
