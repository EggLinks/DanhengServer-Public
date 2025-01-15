using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.TrainParty;

public class PacketTrainPartyGetDataScRsp : BasePacket
{
    public PacketTrainPartyGetDataScRsp(PlayerInstance player) : base(CmdIds.TrainPartyGetDataScRsp)
    {
        var proto = new TrainPartyGetDataScRsp
        {
            TrainPartyData = player.TrainPartyManager!.ToProto()
        };

        SetData(proto);
    }
}