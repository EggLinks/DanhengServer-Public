using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SetHeroBasicTypeCsReq)]
    public class HandlerSetHeroBasicTypeCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SetHeroBasicTypeCsReq.Parser.ParseFrom(data);

            var player = connection.Player!;
            var avatar = player.AvatarManager!.GetHero();
            if (avatar == null)
            {
                connection.SendPacket(new PacketSetHeroBasicTypeScRsp());
                return;
            }

            avatar.HeroId = (int)req.BasicType;
            player.Data.CurBasicType = (int)req.BasicType;

            connection.SendPacket(new PacketPlayerSyncScNotify(avatar));
            connection.SendPacket(new PacketSetHeroBasicTypeScRsp((uint)req.BasicType));
        }
    }
}
