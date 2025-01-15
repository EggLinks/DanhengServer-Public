using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.TrainParty;

public class PacketTrainPartyAddBuildDynamicBuffScRsp : BasePacket
{
    public PacketTrainPartyAddBuildDynamicBuffScRsp() : base(CmdIds.TrainPartyAddBuildDynamicBuffScRsp)
    {
        var proto = new TrainPartyAddBuildDynamicBuffScRsp
        {
            BuffId = 102
        };

        SetData(proto);
    }
}