using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Database.Mail
{
    [SugarTable("Mail")]
    public class MailData : BaseDatabaseDataHelper
    {
        [SugarColumn(IsJson = true)]
        public List<MailInfo> MailList { get; set; } = [];

        [SugarColumn(IsJson = true)]
        public List<MailInfo> NoticeMailList { get; set; } = [];

        public int NextMailId { get; set; } = 1;
    }

    public class MailInfo
    {
        public int MailID { get; set; }
        public string SenderName { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public bool IsRead { get; set; }
        public bool IsStar { get; set; }
        public long SendTime { get; set; }
        public long ExpireTime { get; set; }
        public int TemplateID { get; set; }
        public MailAttachmentInfo Attachment { get; set; } = new();

        public ClientMail ToProto()
        {
            return new()
            {
                Id = (uint)MailID,
                Sender = SenderName,
                Content = Content,
                MailType = IsStar ? MailType.Star : MailType.Normal,
                ExpireTime = ExpireTime,
                IsRead = IsRead,
                TemplateId = (uint)TemplateID,
                Title = Title,
                Time = SendTime,
                Attachment = Attachment.ToProto()
            };
        }
    }

    public class MailAttachmentInfo
    {
        public List<ItemData> Items { get; set; } = [];

        public ItemList ToProto()
        {
            return new()
            {
                ItemList_ = { Items.Select(x => x.ToProto()).ToList() }
            };
        }
    }
}
