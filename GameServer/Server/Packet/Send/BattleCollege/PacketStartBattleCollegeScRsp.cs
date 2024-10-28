using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.BattleCollege;

public class PacketStartBattleCollegeScRsp : BasePacket
{
    public PacketStartBattleCollegeScRsp(uint id, Retcode retCode, BattleInstance? instance) : base(
        CmdIds.StartBattleCollegeScRsp)
    {
        var proto = new StartBattleCollegeScRsp
        {
            Retcode = (uint)retCode,
            Id = id
        };

        if (instance != null)
            proto.BattleInfo = instance.ToProto();

        SetData(proto);
    }
}