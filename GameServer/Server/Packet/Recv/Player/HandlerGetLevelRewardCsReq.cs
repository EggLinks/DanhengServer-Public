using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.GetLevelRewardCsReq)]
public class HandlerGetLevelRewardCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetLevelRewardCsReq.Parser.ParseFrom(data);

        var player = connection.Player!;
        if (player.Data.TakenLevelReward.Contains((int)req.Level))
        {
            await connection.SendPacket(new PacketGetLevelRewardScRsp(Retcode.RetLevelRewardHasTaken));
            return;
        }

        if (player.Data.Level < req.Level)
        {
            await connection.SendPacket(new PacketGetLevelRewardScRsp(Retcode.RetLevelRewardLevelError));
            return;
        }

        if (!GameData.PlayerLevelConfigData.TryGetValue((int)req.Level, out var levelExcel))
        {
            await connection.SendPacket(new PacketGetLevelRewardScRsp(Retcode.RetLevelRewardLevelError));
            return;
        }

        player.Data.TakenLevelReward.Add((int)req.Level);
        var rewards = await player.InventoryManager!.HandleReward(levelExcel.LevelRewardID);
        await connection.SendPacket(new PacketGetLevelRewardScRsp(req.Level, rewards));
    }
}