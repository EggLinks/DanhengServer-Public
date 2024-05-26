using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Gacha
{
    public class PacketGetFriendListInfoScRsp : BasePacket
    {
        public PacketGetFriendListInfoScRsp(Connection connection) : base(CmdIds.GetFriendListInfoScRsp)
        {
            SetData(connection.Player!.FriendManager!.ToProto());
        }
    }
}
