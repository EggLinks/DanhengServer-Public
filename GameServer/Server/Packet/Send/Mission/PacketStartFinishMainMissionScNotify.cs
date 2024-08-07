using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketStartFinishMainMissionScNotify : BasePacket
{
    public PacketStartFinishMainMissionScNotify(int missionId) : base(CmdIds.StartFinishMainMissionScNotify)
    {
        var proto = new StartFinishMainMissionScNotify
        {
            MainMissionId = (uint)missionId
        };

        SetData(proto);
    }
}