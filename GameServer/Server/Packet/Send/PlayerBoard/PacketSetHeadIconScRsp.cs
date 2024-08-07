using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;

public class PacketSetHeadIconScRsp : BasePacket
{
    public PacketSetHeadIconScRsp(PlayerInstance player) : base(CmdIds.SetHeadIconScRsp)
    {
        var proto = new SetHeadIconScRsp
        {
            CurrentHeadIconId = (uint)player.Data.HeadIcon
        };
        SetData(proto);
    }
}