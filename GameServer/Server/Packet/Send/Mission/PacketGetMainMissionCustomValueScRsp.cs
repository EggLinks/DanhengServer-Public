using EggLink.DanhengServer.Enums.Mission;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketGetMainMissionCustomValueScRsp : BasePacket
{
    public PacketGetMainMissionCustomValueScRsp(GetMainMissionCustomValueCsReq req, PlayerInstance player) : base(
        CmdIds.GetMainMissionCustomValueScRsp)
    {
        var proto = new GetMainMissionCustomValueScRsp();
        foreach (var mission in req.MainMissionIdList)
            proto.MainMissionList.Add(new MainMission
            {
                Id = mission,
                Status = player.MissionManager!.GetMainMissionStatus((int)mission).ToProto()
            });

        SetData(proto);
    }
}