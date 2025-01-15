using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

namespace EggLink.DanhengServer.GameServer.Game.Mission.FinishAction.Handler;

[MissionFinishAction(FinishActionTypeEnum.SetFloorSavedValue)]
public class MissionHandlerSetFloorSavedValue : MissionFinishActionHandler
{
    public override async ValueTask OnHandle(List<int> Params, List<string> ParamString, PlayerInstance Player)
    {
        _ = int.TryParse(ParamString[0], out var plane);
        _ = int.TryParse(ParamString[1], out var floor);
        Player.SceneData!.FloorSavedData.TryGetValue(floor, out var value);
        if (value == null)
        {
            value = [];
            Player.SceneData.FloorSavedData[floor] = value;
        }

        value[ParamString[2]] = int.Parse(ParamString[3]); // ParamString[2] is the key
        await Player.SendPacket(
            new PacketUpdateFloorSavedValueNotify(ParamString[2], int.Parse(ParamString[3]), Player));

        Player.TaskManager?.SceneTaskTrigger.TriggerFloor(plane, floor);
    }
}