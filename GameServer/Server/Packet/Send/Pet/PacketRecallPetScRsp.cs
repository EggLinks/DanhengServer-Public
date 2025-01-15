using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Pet;

public class PacketRecallPetScRsp : BasePacket
{
    public PacketRecallPetScRsp(uint newPetId) : base(CmdIds.RecallPetScRsp)
    {
        var proto = new RecallPetScRsp
        {
            CurPetId = newPetId,
            SelectPetId = 0
        };

        SetData(proto);
    }
}