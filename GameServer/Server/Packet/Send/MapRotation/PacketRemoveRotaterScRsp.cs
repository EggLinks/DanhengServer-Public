using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.MapRotation;

public class PacketRemoveRotaterScRsp : BasePacket
{
    public PacketRemoveRotaterScRsp(PlayerInstance player, RemoveRotaterCsReq req) : base(CmdIds.RemoveRotaterScRsp)
    {
        var proto = new RemoveRotaterScRsp
        {
            EnergyInfo = new RotaterEnergyInfo
            {
                CurNum = (uint)player.ChargerNum,
                MaxNum = 5
            },
            RotaterData = req.RotaterData
        };

        SetData(proto);
    }
}