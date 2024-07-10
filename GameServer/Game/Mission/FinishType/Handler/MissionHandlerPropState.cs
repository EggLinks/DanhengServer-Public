using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Scene.Entity;

namespace EggLink.DanhengServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.PropState)]
    public class MissionHandlerPropState : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var prop = player.SceneInstance?.GetEntitiesInGroup<EntityProp>(info.ParamInt1);
            if (prop == null) return;
            
            foreach (var p in prop)
            {
                if (p.PropInfo.ID == info.ParamInt2)
                {
                    //if (player.SceneInstance?.Excel.WorldID != 101)
                    //{
                    //    if (p.PropInfo.State == PropStateEnum.Locked && info.SourceState == PropStateEnum.Closed)
                    //    {
                    //        GameData.MazePropData.TryGetValue(p.PropInfo.PropID, out var propData);
                    //        if (propData != null && propData.PropStateList.Contains(PropStateEnum.Closed))
                    //        {
                    //            p.SetState(PropStateEnum.Closed);
                    //        }
                    //        else
                    //        {
                    //        }
                    //    }
                    //} else
                    //{
                    //    p.SetState(info.SourceState);
                    //}
                    GameData.MazePropData.TryGetValue(p.PropInfo.PropID, out var data);
                    if (data?.PropStateList.Contains(info.SourceState) == true)
                    {
                        p.SetState(info.SourceState);
                    }
                }
            }
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            if (player.SceneInstance?.FloorId != info.LevelFloorID) return;  // not a same scene
            var prop = player.SceneInstance.GetEntitiesInGroup<EntityProp>(info.ParamInt1);
            if (prop == null) return;

            foreach (var p in prop)
            {
                if (p.PropInfo.ID == info.ParamInt2 && (int)p.State == info.ParamInt3)
                {
                    player.MissionManager!.FinishSubMission(info.ID);
                } 
                else if (info.ParamInt3 == (int)PropStateEnum.CheckPointDisable || info.ParamInt3 == (int)PropStateEnum.CheckPointEnable)
                {
                    player.MissionManager!.FinishSubMission(info.ID);
                }
            }
        }
    }
}
