using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Activity;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Activity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Activity;

[Opcode(CmdIds.TakeTrialActivityRewardCsReq)]
public class HandlerTakeTrialActivityRewardCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = TakeTrialActivityRewardCsReq.Parser.ParseFrom(data);

        GameData.AvatarDemoConfigData.TryGetValue((int)req.StageId, out var stage);
        if (stage != null)
        {
            GameData.RewardDataData.TryGetValue(stage.RewardID, out var reward);
            var itemList = new List<ItemData>();
            reward?.GetItems().ForEach(i =>
            {
                var res = connection.Player!.InventoryManager!.AddItem(i.Item1, i.Item2, false).Result;
                if (res != null) itemList.Add(res);
            });
            var activities = connection.Player!.ActivityManager!.Data.TrialActivityData.Activities;
            var activity = activities.Find(x => x.StageId == req.StageId);
            if (activity != null)
                activities[activities.FindIndex(x => x.StageId == req.StageId)] = new TrialActivityResultData
                {
                    StageId = (int)req.StageId,
                    TakenReward = true
                };
            connection.Player!.Data.Hcoin += reward!.Hcoin;

            await connection.Player!.SendPacket(new PacketPlayerSyncScNotify(connection.Player!.ToProto(), itemList));
            await connection.Player!.SendPacket(new PacketScenePlaneEventScNotify(itemList));
            await connection.SendPacket(new PacketTakeTrialActivityRewardScRsp(req.StageId, itemList));
        }
    }
}