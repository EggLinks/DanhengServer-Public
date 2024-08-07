using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Database.Mail;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Mail;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Mail;

public class MailManager(PlayerInstance player) : BasePlayerManager(player)
{
    public MailData MailData { get; } = DatabaseHelper.Instance!.GetInstanceOrCreateNew<MailData>(player.Uid);

    public List<MailInfo> GetMailList()
    {
        return MailData.MailList;
    }

    public MailInfo? GetMail(int mailId)
    {
        return MailData.MailList.Find(x => x.MailID == mailId);
    }

    public void DeleteMail(int mailId)
    {
        var index = MailData.MailList.FindIndex(x => x.MailID == mailId);
        MailData.MailList.RemoveAt(index);
    }

    public async ValueTask SendMail(string sender, string title, string content, int templateId, int expiredDay = 30)
    {
        var mail = new MailInfo
        {
            MailID = MailData.NextMailId++,
            SenderName = sender,
            Content = content,
            Title = title,
            TemplateID = templateId,
            SendTime = DateTime.Now.ToUnixSec(),
            ExpireTime = DateTime.Now.AddDays(expiredDay).ToUnixSec()
        };

        MailData.MailList.Add(mail);

        await Player.SendPacket(new PacketNewMailScNotify(mail.MailID));
    }

    public async ValueTask SendMail(string sender, string title, string content, int templateId,
        List<ItemData> attachments, int expiredDay = 30)
    {
        var mail = new MailInfo
        {
            MailID = MailData.NextMailId++,
            SenderName = sender,
            Content = content,
            Title = title,
            TemplateID = templateId,
            SendTime = DateTime.Now.ToUnixSec(),
            ExpireTime = DateTime.Now.AddDays(expiredDay).ToUnixSec(),
            Attachment = new MailAttachmentInfo
            {
                Items = attachments
            }
        };

        MailData.MailList.Add(mail);

        await Player.SendPacket(new PacketNewMailScNotify(mail.MailID));
    }

    public List<ClientMail> ToMailProto()
    {
        var list = new List<ClientMail>();

        foreach (var mail in MailData.MailList) list.Add(mail.ToProto());

        return list;
    }

    public List<ClientMail> ToNoticeMailProto()
    {
        var list = new List<ClientMail>();

        foreach (var mail in MailData.NoticeMailList) list.Add(mail.ToProto());

        return list;
    }
}