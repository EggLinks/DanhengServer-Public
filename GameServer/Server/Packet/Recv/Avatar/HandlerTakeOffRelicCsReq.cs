using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.TakeOffRelicCsReq)]
    public class HandlerTakeOffRelicCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = TakeOffRelicCsReq.Parser.ParseFrom(data);
            foreach (var param in req.RelicTypeList)
            {
                connection.Player!.InventoryManager!.UnequipRelic((int)req.DressAvatarId, (int)param);
            }
            connection.SendPacket(CmdIds.TakeOffRelicScRsp);
        }
    }
}
