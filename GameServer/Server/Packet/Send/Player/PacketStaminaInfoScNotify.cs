using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketStaminaInfoScNotify : BasePacket
{
    public PacketStaminaInfoScNotify(PlayerInstance player) : base(CmdIds.StaminaInfoScNotify)
    {
        var proto = new StaminaInfoScNotify
        {
            Stamina = (uint)player.Data.Stamina,
            ReserveStamina = (uint)player.Data.StaminaReserve,
            NextRecoverTime = player.Data.NextStaminaRecover
        };

        SetData(proto);
    }
}