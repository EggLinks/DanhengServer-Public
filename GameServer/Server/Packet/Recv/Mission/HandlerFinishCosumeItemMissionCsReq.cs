using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Mission;

[Opcode(CmdIds.FinishCosumeItemMissionCsReq)]
public class HandlerFinishCosumeItemMissionCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = FinishCosumeItemMissionCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;
        var mission = player.MissionManager?.GetSubMissionInfo((int)req.SubMissionId);
        if (mission == null)
        {
            await connection.SendPacket(new PacketFinishCosumeItemMissionScRsp());
            return;
        }

        mission.ParamItemList?.ForEach(async param =>
        {
            await player.InventoryManager!.RemoveItem(param.ItemID, param.ItemNum);
        });

        await player.MissionManager!.FinishSubMission((int)req.SubMissionId);

        await connection.SendPacket(new PacketFinishCosumeItemMissionScRsp(req.SubMissionId));
    }
}