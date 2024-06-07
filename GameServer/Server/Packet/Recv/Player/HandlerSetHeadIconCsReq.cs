using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SetHeadIconCsReq)]
    public class HandlerSetHeadIconCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            var req = SetHeadIconCsReq.Parser.ParseFrom(data);
            if (req == null) return;
            player.Data.HeadIcon = (int)req.Id;
            DatabaseHelper.Instance?.UpdateInstance(player.Data);
            connection.SendPacket(new PacketSetHeadIconScRsp(player));
        }
    }
}
