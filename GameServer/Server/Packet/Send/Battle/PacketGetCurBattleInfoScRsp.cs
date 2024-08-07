using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle;

public class PacketGetCurBattleInfoScRsp : BasePacket
{
    public PacketGetCurBattleInfoScRsp() : base(CmdIds.GetCurBattleInfoScRsp)
    {
        var proto = new GetCurBattleInfoScRsp
        {
            BattleInfo = new SceneBattleInfo()
        };

        SetData(proto);
    }
}