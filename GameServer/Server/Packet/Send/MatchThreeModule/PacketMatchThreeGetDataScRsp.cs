using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.MatchThreeModule;

public class PacketMatchThreeGetDataScRsp : BasePacket
{
    public PacketMatchThreeGetDataScRsp(PlayerInstance player) : base(CmdIds.MatchThreeGetDataScRsp)
    {
        var proto = new MatchThreeGetDataScRsp
        {
            MatchThreeData = player.MatchThreeManager!.ToProto()
        };

        SetData(proto);
    }
}