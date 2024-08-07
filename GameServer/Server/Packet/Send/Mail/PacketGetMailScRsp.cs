using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;

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