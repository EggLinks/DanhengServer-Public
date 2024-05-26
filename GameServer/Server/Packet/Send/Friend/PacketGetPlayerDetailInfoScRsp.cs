using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Friend
{
    public class PacketGetPlayerDetailInfoScRsp : BasePacket
    {
        public PacketGetPlayerDetailInfoScRsp(PlayerData data) : base(CmdIds.GetPlayerDetailInfoScRsp)
        {
            var proto = new GetPlayerDetailInfoScRsp()
            {
                DetailInfo = data.ToDetailProto(),
            };

            SetData(proto);
        }

        public PacketGetPlayerDetailInfoScRsp() : base(CmdIds.GetPlayerDetailInfoScRsp)
        {
            var proto = new GetPlayerDetailInfoScRsp()
            {
                Retcode = 3612,
            };

            SetData(proto);
        }
    }
}
