using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketMissionAcceptScNotify : BasePacket
{
    public PacketMissionAcceptScNotify(int missionId) : this([missionId])
    {
    }

    public PacketMissionAcceptScNotify(List<int> missionIds) : base(CmdIds.MissionAcceptScNotify)
    {
        var proto = new MissionAcceptScNotify();
        foreach (var missionId in missionIds) proto.SubMissionIdList.Add((uint)missionId);

        SetData(proto);
    }
}