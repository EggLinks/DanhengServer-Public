using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Gacha
{
    public class PacketGetFriendApplyListInfoCsReq : BasePacket
    {
        public PacketGetFriendApplyListInfoCsReq(Connection connection) : base(CmdIds.GetFriendApplyListInfoScRsp)
        {

            SetData(connection.Player!.FriendManager!.ToApplyListProto());
        }
    }
}
