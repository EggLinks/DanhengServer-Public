using EggLink.DanhengServer.GameServer.Game.Battle;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;

public class PacketSceneEnterStageScRsp : BasePacket
{
    public PacketSceneEnterStageScRsp() : base(CmdIds.SceneEnterStageScRsp)
    {
        var proto = new SceneEnterStageScRsp
        {
            Retcode = 1
        };

        SetData(proto);
    }

    public PacketSceneEnterStageScRsp(BattleInstance battleInstance) : base(CmdIds.SceneEnterStageScRsp)
    {
        var proto = new SceneEnterStageScRsp
        {
            BattleInfo = battleInstance.ToProto()
        };

        SetData(proto);
    }
}