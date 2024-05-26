using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketTakePromotionRewardScRsp : BasePacket
    {
        public PacketTakePromotionRewardScRsp() : base(CmdIds.TakePromotionRewardScRsp)
        {
            var itemList = new Item()
            {
                ItemId = 101,
                Num = 1
            };

            var rsp = new TakePromotionRewardScRsp
            {
                RewardList = new()
                {
                    ItemList_ = { itemList }
                }
            };

            SetData(rsp);
        }
    }
}
