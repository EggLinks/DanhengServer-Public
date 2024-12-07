using EggLink.DanhengServer.GameServer.Game.RogueTourn.Scene;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketRogueWorkbenchGetInfoScRsp : BasePacket
{
    public PacketRogueWorkbenchGetInfoScRsp(Retcode ret, RogueWorkbenchProp? prop) : base(
        CmdIds.RogueWorkbenchGetInfoScRsp)
    {
        var proto = new RogueWorkbenchGetInfoScRsp
        {
            Retcode = (uint)ret
        };

        if (prop != null)
            foreach (var rogueWorkbenchFunc in prop.WorkbenchFuncs)
                proto.FuncInfoMap.Add((uint)rogueWorkbenchFunc.FuncId, rogueWorkbenchFunc.ToProto());

        SetData(proto);
    }
}