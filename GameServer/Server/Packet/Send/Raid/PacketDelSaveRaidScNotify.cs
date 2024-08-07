using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Raid;

public class PacketDelSaveRaidScNotify : BasePacket
{
    public PacketDelSaveRaidScNotify(int raidId, int worldLevel) : base(CmdIds.DelSaveRaidScNotify)
    {
        var proto = new DelSaveRaidScNotify
        {
            RaidId = (uint)raidId,
            WorldLevel = (uint)worldLevel
        };

        SetData(proto);
    }
}