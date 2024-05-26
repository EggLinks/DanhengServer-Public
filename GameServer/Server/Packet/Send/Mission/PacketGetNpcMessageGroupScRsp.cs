using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketGetNpcMessageGroupScRsp : BasePacket
    { 
        public PacketGetNpcMessageGroupScRsp(IEnumerable<uint> contactIdList, PlayerInstance instance) : base(CmdIds.GetNpcMessageGroupScRsp)
        {
            var proto = new GetNpcMessageGroupScRsp();

            foreach (var contactId in contactIdList)
            {
                var contact = instance.MessageManager!.GetMessageGroup((int)contactId);

                proto.MessageGroupList.AddRange(contact);
            }

            SetData(proto);
        }
    }
}
