using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketActivateFarmElementScRsp : BasePacket
{
    public PacketActivateFarmElementScRsp(uint entityId, PlayerInstance player) : base(CmdIds.ActivateFarmElementScRsp)
    {
        var proto = new ActivateFarmElementScRsp
        {
            EntityId = entityId,
            WorldLevel = (uint)player.Data.WorldLevel
        };

        SetData(proto);
    }
}