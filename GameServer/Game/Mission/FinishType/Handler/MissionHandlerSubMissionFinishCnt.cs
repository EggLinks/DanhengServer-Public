using EggLink.DanhengServer.Data.Config;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Game.Mission.FinishType.Handler
{
    [MissionFinishType(MissionFinishTypeEnum.SubMissionFinishCnt)]
    public class MissionHandlerSubMissionFinishCnt : MissionFinishTypeHandler
    {
        public override void Init(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            // Do nothing
        }

        public override void HandleFinishType(PlayerInstance player, SubMissionInfo info, object? arg)
        {
            var finish = info.Operation == OperationEnum.And;
            var finishCount = 0;
            foreach (var missionId in info.ParamIntList ?? [])
            {
                var status = player.MissionManager!.GetSubMissionStatus(missionId);
                if (status != MissionPhaseEnum.Finish && status != MissionPhaseEnum.Cancel)
                {
                    if (info.Operation == OperationEnum.And)
                    {
                        finish = false;
                    }
                } else if (status == MissionPhaseEnum.Finish || status == MissionPhaseEnum.Cancel)
                {
                    finishCount++;
                    if (info.Operation == OperationEnum.Or)
                    {
                        finish = true;
                        break;
                    }
                }
            }
            if (finish)
            {
                player.MissionManager!.FinishSubMission(info.ID);
            } else
            {
                if (finishCount > 0)
                {
                    var sync = new Proto.MissionSync()
                    {
                        MissionList =
                    {
                        new Proto.Mission()
                        {
                            Id = (uint)info.ID,
                            Status = Proto.MissionStatus.MissionDoing,
                            Progress = (uint)finishCount
                        }
                    }
                    };

                    player.SendPacket(new PacketPlayerSyncScNotify(sync));
                }
            }
        }
    }
}
