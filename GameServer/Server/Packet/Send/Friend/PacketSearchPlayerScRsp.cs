using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Friend
{
    public class PacketSearchPlayerScRsp : BasePacket
    {
        public PacketSearchPlayerScRsp() : base(CmdIds.SearchPlayerScRsp)
        {
            var proto = new SearchPlayerScRsp()
            {
                Retcode = 3612
            };

            SetData(proto);
        }

        public PacketSearchPlayerScRsp(List<PlayerData> data) : base(CmdIds.SearchPlayerScRsp)
        {
            var proto = new SearchPlayerScRsp();

            proto.ResultUidList.AddRange(data.Select(x => (uint)x.Uid));
            proto.SimpleInfoList.AddRange(data.Select(x => x.ToSimpleProto(FriendOnlineStatus.Online)));

            SetData(proto);
        }
    }
}
