using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketStartFinishSubMissionScNotify : BasePacket
{
    public PacketStartFinishSubMissionScNotify(int missionId) : base(CmdIds.StartFinishSubMissionScNotify)
    {
        var proto = new StartFinishSubMissionScNotify
        {
            SubMissionId = (uint)missionId
        };

        SetData(proto);
    }
}