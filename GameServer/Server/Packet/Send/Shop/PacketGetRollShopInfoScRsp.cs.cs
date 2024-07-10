using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Proto;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Shop
{
    public class PacketGetRollShopInfoScRsp : BasePacket
    {
        public PacketGetRollShopInfoScRsp(uint rollShopId) : base(CmdIds.GetRollShopInfoScRsp)
        {
            var proto = new GetRollShopInfoScRsp();

            proto.RollShopId = rollShopId;
            proto.GachaRandom = 1;

            foreach (var item in GameData.RollShopConfigData.Values)
            {
                if (item.RollShopID == rollShopId)
                {
                    proto.NOPNEOADJEI.Add(item.T1GroupID);
                    proto.NOPNEOADJEI.Add(item.T2GroupID);
                    proto.NOPNEOADJEI.Add(item.T3GroupID);
                    proto.NOPNEOADJEI.Add(item.T4GroupID);
                }
            }

            proto.Retcode = 0;

            SetData(proto);
        }
    }
}
