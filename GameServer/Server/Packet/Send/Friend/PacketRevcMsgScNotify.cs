using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Friend
{
    public class PacketRevcMsgScNotify : BasePacket
    {
        public PacketRevcMsgScNotify(uint toUid, uint fromUid, string msg) : base(CmdIds.RevcMsgScNotify)
        {
            RevcMsgScNotify proto = new RevcMsgScNotify()
            {
                ChatType = ChatType.Private,
                SourceUid = fromUid,
                TargetUid = toUid,
                MessageText = msg,
                MessageType = MsgType.CustomText,
            };

            SetData(proto);
        }

        public PacketRevcMsgScNotify(uint toUid, uint fromUid, uint extraId) : base(CmdIds.RevcMsgScNotify)
        {
            RevcMsgScNotify proto = new RevcMsgScNotify()
            {
                ChatType = ChatType.Private,
                SourceUid = fromUid,
                TargetUid = toUid,
                ExtraId = extraId,
                MessageType = MsgType.Emoji
            };

            SetData(proto);
        }
    }
}
