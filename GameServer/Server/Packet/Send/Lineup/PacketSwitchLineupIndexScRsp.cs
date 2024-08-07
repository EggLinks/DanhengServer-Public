using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Lineup;

public class PacketSwitchLineupIndexScRsp : BasePacket
{
    public PacketSwitchLineupIndexScRsp(uint index) : base(CmdIds.SwitchLineupIndexScRsp)
    {
        var proto = new SwitchLineupIndexScRsp
        {
            Index = index
        };

        SetData(proto);
    }
}