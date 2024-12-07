using EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketRogueWorkbenchHandleFuncScRsp : BasePacket
{
    public PacketRogueWorkbenchHandleFuncScRsp(Retcode retcode, uint funcId, RogueWorkbenchFunc? func) : base(
        CmdIds.RogueWorkbenchHandleFuncScRsp)
    {
        var proto = new RogueWorkbenchHandleFuncScRsp
        {
            Retcode = (uint)retcode,
            WorkbenchFuncId = funcId
        };

        if (func != null) proto.TargetFuncInfo = func.ToProto();

        SetData(proto);
    }
}