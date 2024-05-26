using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketGetAssistHistoryScRsp : BasePacket
    {
        public PacketGetAssistHistoryScRsp(PlayerInstance player) : base(CmdIds.GetAssistHistoryScRsp)
        {
            var proto = new GetAssistHistoryScRsp
            {
            };

            SetData(proto);
        }
    }
}
