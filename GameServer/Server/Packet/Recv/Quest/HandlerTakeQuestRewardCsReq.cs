using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Quest;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Quest;

[Opcode(CmdIds.TakeQuestRewardCsReq)]
public class HandlerTakeQuestRewardCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = TakeQuestRewardCsReq.Parser.ParseFrom(data);
        List<ItemData> rewardItems = [];
        var ret = Retcode.RetSucc;
        List<int> succQuestIds = [];

        foreach (var quest in req.QuestIdList)
        {
            var (retCode, items) = await connection.Player!.QuestManager!.TakeQuestReward((int)quest);
            if (retCode != Retcode.RetSucc)
                ret = retCode;
            else
                succQuestIds.Add((int)quest);

            if (items != null) rewardItems.AddRange(items);
        }

        await connection.SendPacket(new PacketTakeQuestRewardScRsp(ret, rewardItems, succQuestIds));
    }
}