using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Quest
{
    public class PacketGetQuestDataScRsp : BasePacket
    {
        public PacketGetQuestDataScRsp() : base(CmdIds.GetQuestDataScRsp)
        {
            var proto = new GetQuestDataScRsp();
            foreach (var quest in GameData.QuestDataData.Values)
            {
                proto.QuestList.Add(new Proto.Quest()
                {
                    Id = (uint)quest.QuestID,
                    Status = QuestStatus.QuestClose
                });
            }
            SetData(proto);
        }
    }
}
