using EggLink.DanhengServer.GameServer.Server.Packet.Send.TrainParty;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.TrainParty;

[Opcode(CmdIds.TrainPartyBuildDiyCsReq)]
public class HandlerTrainPartyBuildDiyCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = TrainPartyBuildDiyCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        var area = player.TrainPartyManager!.SetDynamicId((int)req.AreaId, (int)req.DiceSlotId, (int)req.DiyDynamicId);
        await player.SendPacket(new PacketTrainPartyBuildDiyScRsp(area));
    }
}