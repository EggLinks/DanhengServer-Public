using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketExpUpEquipmentScRsp : BasePacket
    {
        public PacketExpUpEquipmentScRsp(List<ItemData> returnItem) : base(CmdIds.ExpUpEquipmentScRsp)
        {
            var proto = new ExpUpEquipmentScRsp();
            proto.ReturnItemList.AddRange(returnItem.Select(item => item.ToPileProto()));

            SetData(proto);
        }
    }
}
