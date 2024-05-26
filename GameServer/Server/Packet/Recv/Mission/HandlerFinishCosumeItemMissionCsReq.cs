using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Mission
{
    [Opcode(CmdIds.FinishCosumeItemMissionCsReq)]
    public class HandlerFinishCosumeItemMissionCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = FinishCosumeItemMissionCsReq.Parser.ParseFrom(data);

            var player = connection.Player!;
            var mission = player.MissionManager?.GetSubMissionInfo((int)req.SubMissionId);
            if (mission == null)
            {
                connection.SendPacket(new PacketFinishCosumeItemMissionScRsp());
                return;
            }

            mission.ParamItemList?.ForEach(param =>
            {
                player.InventoryManager?.RemoveItem(param.ItemID, param.ItemNum);
            });

            player.MissionManager?.FinishSubMission((int)req.SubMissionId);

            connection.SendPacket(new PacketFinishCosumeItemMissionScRsp(req.SubMissionId));
        }
    }
}
