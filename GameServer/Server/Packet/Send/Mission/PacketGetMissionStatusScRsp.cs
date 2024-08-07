using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketGetMissionStatusScRsp : BasePacket
{
    public PacketGetMissionStatusScRsp(GetMissionStatusCsReq req, PlayerInstance player) : base(
        CmdIds.GetMissionStatusScRsp)
    {
        var proto = new GetMissionStatusScRsp();

        foreach (var item in req.MainMissionIdList)
        {
            var status = player.MissionManager!.GetMainMissionStatus((int)item);
            if (status == MissionPhaseEnum.Finish)
                proto.FinishedMainMissionIdList.Add(item);
            else
                proto.UnfinishedMainMissionIdList.Add(item);
        }

        foreach (var item in req.SubMissionIdList)
        {
            var status = player.MissionManager!.GetSubMissionStatus((int)item);
            proto.SubMissionStatusList.Add(new Proto.Mission
            {
                Id = item,
                Status = status.ToProto()
            });
        }

        SetData(proto);
    }
}