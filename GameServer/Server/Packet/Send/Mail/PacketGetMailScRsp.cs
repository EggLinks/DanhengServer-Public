using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail
{
    public class PacketGetMailScRsp : BasePacket
    {
        public PacketGetMailScRsp(PlayerInstance player) : base(CmdIds.GetMailScRsp)
        {
            var list = player.MailManager!.ToMailProto();
            var noticeList = player.MailManager!.ToNoticeMailProto();
            var proto = new GetMailScRsp
            {
                IsEnd = true,
                MailList = { list },
                NoticeMailList = { noticeList },
                TotalNum = (uint)(list.Count + noticeList.Count)
            };

            SetData(proto);
        }
    }
}
