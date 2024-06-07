using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketGetFarmStageGachaInfoScRsp : BasePacket
    {
        public PacketGetFarmStageGachaInfoScRsp(GetFarmStageGachaInfoCsReq req) : base(CmdIds.GetFarmStageGachaInfoScRsp)
        {
            var proto = new GetFarmStageGachaInfoScRsp();

            foreach (var item in req.FarmStageGachaIdList)
            {
                proto.FarmStageGachaInfoList.Add(new FarmStageGachaInfo()
                {
                    GachaId = item,
                    BeginTime = 0,
                    EndTime = long.MaxValue
                });
            }

            SetData(proto);
        }
    }
}
