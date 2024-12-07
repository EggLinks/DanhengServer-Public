using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Pet;

public class PacketCurPetChangedScNotify : BasePacket
{
    public PacketCurPetChangedScNotify(uint newPetId) : base(CmdIds.CurPetChangedScNotify)
    {
        var proto = new CurPetChangedScNotify
        {
            CurPetId = newPetId
        };

        SetData(proto);
    }
}