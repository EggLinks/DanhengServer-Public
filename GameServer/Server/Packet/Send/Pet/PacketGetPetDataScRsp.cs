using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Pet;

public class PacketGetPetDataScRsp : BasePacket
{
    public PacketGetPetDataScRsp(PlayerInstance player) : base(CmdIds.GetPetDataScRsp)
    {
        var proto = new GetPetDataScRsp
        {
            CurPetId = (uint)player.Data.Pet
        };

        foreach (var pet in GameData.PetData.Values) proto.UnlockedPetId.Add((uint)pet.PetID);

        SetData(proto);
    }
}