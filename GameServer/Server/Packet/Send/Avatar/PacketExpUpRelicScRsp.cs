using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketExpUpRelicScRsp : BasePacket
    {
        public PacketExpUpRelicScRsp(List<ItemData> leftover) : base(CmdIds.ExpUpRelicScRsp)
        {
            var proto = new ExpUpRelicScRsp();

            foreach (var item in leftover)
            {
                proto.ReturnItemList.Add(item.ToPileProto());
            }

            SetData(proto);
        }
    }
}
