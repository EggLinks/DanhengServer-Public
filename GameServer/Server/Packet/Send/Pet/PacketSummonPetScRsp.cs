using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Pet;

public class PacketSummonPetScRsp : BasePacket
{
    public PacketSummonPetScRsp(int curPetId, uint newPetId) : base(CmdIds.SummonPetScRsp)
    {
        var proto = new SummonPetScRsp
        {
            CurPetId = (uint)curPetId,
            SelectPetId = newPetId
        };

        SetData(proto);
    }
}