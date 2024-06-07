using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Avatar
{
    [Opcode(CmdIds.SetDisplayAvatarCsReq)]
    public class HandlerSetDisplayAvatarCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SetDisplayAvatarCsReq.Parser.ParseFrom(data);
            var player = connection.Player!;
            var avatars = player.AvatarManager!.AvatarData!.DisplayAvatars;
            avatars.Clear();
            foreach (var id in req.DisplayAvatarList)
            {
                if (id.AvatarId == 0) continue;
                avatars.Add((int)id.AvatarId);
            }
            DatabaseHelper.Instance!.UpdateInstance(player.AvatarManager.AvatarData!);
            connection.SendPacket(new PacketSetDisplayAvatarScRsp(req.DisplayAvatarList));
        }
    }
}
