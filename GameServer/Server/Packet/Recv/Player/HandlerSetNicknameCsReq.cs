using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SetNicknameCsReq)]
    public class HandlerSetNicknameCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var player = connection.Player!;
            var req = SetNicknameCsReq.Parser.ParseFrom(data);
            if (req == null) return;
            player.Data.Name = req.Nickname;
            DatabaseHelper.Instance?.UpdateInstance(player.Data);
            connection.SendPacket(CmdIds.SetNicknameScRsp);
            connection.SendPacket(new PacketPlayerSyncScNotify(player.ToProto()));
        }
    }
}
