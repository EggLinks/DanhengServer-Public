using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RollShop;

public class PacketGetRollShopInfoScRsp : BasePacket
{
    public PacketGetRollShopInfoScRsp(uint rollShopId) : base(CmdIds.GetRollShopInfoScRsp)
    {
        var proto = new GetRollShopInfoScRsp();

        proto.RollShopId = rollShopId;
        proto.GachaRandom = 1;

        foreach (var item in GameData.RollShopConfigData.Values)
            if (item.RollShopID == rollShopId)
            {
                proto.ShopGroupIdList.Add(item.T1GroupID);
                proto.ShopGroupIdList.Add(item.T2GroupID);
                proto.ShopGroupIdList.Add(item.T3GroupID);
                proto.ShopGroupIdList.Add(item.T4GroupID);
            }

        proto.Retcode = 0;

        SetData(proto);
    }
}