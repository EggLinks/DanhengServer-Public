using EggLink.DanhengServer.GameServer.Game.RogueMagic.Adventure;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketPrepareRogueAdventureRoomScRsp : BasePacket
{
    public PacketPrepareRogueAdventureRoomScRsp(RogueMagicAdventureInstance instance) : base(
        CmdIds.PrepareRogueAdventureRoomScRsp)
    {
        var proto = new PrepareRogueAdventureRoomScRsp
        {
            AdventureRoomInfo = instance.ToProto()
        };

        SetData(proto);
    }


    public PacketPrepareRogueAdventureRoomScRsp(Retcode retcode) : base(CmdIds.PrepareRogueAdventureRoomScRsp)
    {
        var proto = new PrepareRogueAdventureRoomScRsp
        {
            Retcode = (uint)retcode
        };

        SetData(proto);
    }
}