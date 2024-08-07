using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;

public class PacketAcceptMainMissionScRsp : BasePacket
{
    public PacketAcceptMainMissionScRsp(uint missionId) : base(CmdIds.AcceptMainMissionScRsp)
    {
        var proto = new AcceptMainMissionScRsp
        {
            MainMissionId = missionId
        };

        SetData(proto);
    }
}