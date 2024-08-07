using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketEnhanceRogueBuffScRsp : BasePacket
{
    public PacketEnhanceRogueBuffScRsp(uint buffId) : base(CmdIds.EnhanceRogueBuffScRsp)
    {
        var proto = new EnhanceRogueBuffScRsp
        {
            RogueBuff = new RogueBuff
            {
                BuffId = buffId,
                Level = 2
            },
            IsSuccess = true
        };

        SetData(proto);
    }
}