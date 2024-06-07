using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketAvatarExpUpScRsp : BasePacket
    {
        public PacketAvatarExpUpScRsp(List<ItemData> returnItem) : base(CmdIds.AvatarExpUpScRsp)
        {
            var proto = new AvatarExpUpScRsp();
            proto.ReturnItemList.AddRange(returnItem.Select(item => item.ToPileProto()));

            SetData(proto);
        }
    }
}
