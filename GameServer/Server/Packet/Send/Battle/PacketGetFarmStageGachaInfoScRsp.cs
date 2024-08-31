using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle;

public class PacketGetFarmStageGachaInfoScRsp : BasePacket
{
    public PacketGetFarmStageGachaInfoScRsp(GetFarmStageGachaInfoCsReq req) : base(CmdIds.GetFarmStageGachaInfoScRsp)
    {
        var proto = new GetFarmStageGachaInfoScRsp();

        foreach (var item in req.FarmStageGachaIdList)
            proto.FarmStageGachaInfoList.Add(new FarmStageGachaInfo
            {
                GachaId = item,
                BeginTime = 0,
                EndTime = Extensions.GetUnixSec() + 3600
            });

        SetData(proto);
    }
}