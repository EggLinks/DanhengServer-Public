using EggLink.DanhengServer.Enums;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketGetMainMissionCustomValueScRsp : BasePacket
    {
        public PacketGetMainMissionCustomValueScRsp(GetMainMissionCustomValueCsReq req, PlayerInstance player) : base(CmdIds.GetMainMissionCustomValueScRsp)
        {
            var proto = new GetMainMissionCustomValueScRsp();
            foreach (var mission in req.MainMissionIdList)
            {
                proto.MissionDataList.Add(new MissionData
                {
                    Id = mission,
                    Status = player.MissionManager!.GetMainMissionStatus((int)mission).ToProto()
                });
            }

            SetData(proto);
        }
    }
}
