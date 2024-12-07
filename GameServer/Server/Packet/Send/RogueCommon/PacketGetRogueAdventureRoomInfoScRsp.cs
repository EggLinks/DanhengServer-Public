using EggLink.DanhengServer.GameServer.Game.RogueMagic.Adventure;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketGetRogueAdventureRoomInfoScRsp : BasePacket
{
    public PacketGetRogueAdventureRoomInfoScRsp(RogueMagicAdventureInstance instance) : base(
        CmdIds.GetRogueAdventureRoomInfoScRsp)
    {
        var proto = new GetRogueAdventureRoomInfoScRsp
        {
            AdventureRoomInfo = instance.ToProto()
        };

        SetData(proto);
    }

    public PacketGetRogueAdventureRoomInfoScRsp(Retcode retcode) : base(CmdIds.GetRogueAdventureRoomInfoScRsp)
    {
        var proto = new GetRogueAdventureRoomInfoScRsp
        {
            Retcode = (uint)retcode
        };

        SetData(proto);
    }
}