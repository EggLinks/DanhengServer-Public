using EggLink.DanhengServer.Database.ChessRogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketChessRogueNousEditDiceScRsp : BasePacket
{
    public PacketChessRogueNousEditDiceScRsp(ChessRogueNousDiceData diceData) : base(CmdIds.ChessRogueNousEditDiceScRsp)
    {
        var proto = new ChessRogueNousEditDiceScRsp
        {
            QueryDiceInfo = diceData.ToProto()
        };

        SetData(proto);
    }
}