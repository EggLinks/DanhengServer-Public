using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.SetAssistAvatarCsReq)]
    public class HandlerSetAssistAvatarCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SetAssistAvatarCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            var avatars = player.AvatarManager!.AvatarData!.AssistAvatars;
            avatars.Clear();
            foreach (var id in req.AvatarIdList)
            {
                if (id == 0) continue;
                avatars.Add((int)id);
            }
            DatabaseHelper.Instance!.UpdateInstance(player.AvatarManager.AvatarData!);
            connection.SendPacket(new PacketSetAssistAvatarScRsp(req.AvatarIdList));
        }
    }
}
