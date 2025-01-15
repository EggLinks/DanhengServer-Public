using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.MatchThreeModule;

public class PacketMatchThreeLevelEndScRsp : BasePacket
{
    public PacketMatchThreeLevelEndScRsp(uint levelId, uint mode) : base(CmdIds.MatchThreeLevelEndScRsp)
    {
        var proto = new MatchThreeLevelEndScRsp
        {
            LevelId = levelId,
            ModeId = mode
        };

        SetData(proto);
    }
}