using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketGetMissionDataScRsp : BasePacket
    {
        public PacketGetMissionDataScRsp(PlayerInstance player) : base(CmdIds.GetMissionDataScRsp)
        {
            var proto = new GetMissionDataScRsp();
            
            foreach (var mission in GameData.MainMissionData.Keys)
            {
                if (player.MissionManager!.GetMainMissionStatus(mission) == MissionPhaseEnum.Doing)
                {
                    proto.MissionDataList.Add(new MissionData()
                    {
                        Id = (uint)mission,
                        Status = MissionStatus.MissionDoing
                    });
                }
            }

            foreach (var mission in GameData.SubMissionData.Keys)
            {
                if (player.MissionManager!.GetSubMissionStatus(mission) == MissionPhaseEnum.Doing)
                {
                    proto.MissionList.Add(new Proto.Mission()
                    {
                        Id = (uint)mission,
                        Status = MissionStatus.MissionDoing
                    });
                }
            }

            SetData(proto);
        }
    }
}
