using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Avatar;

public class PacketUnlockSkilltreeScRsp : BasePacket
{
    public PacketUnlockSkilltreeScRsp(Retcode retCode) : base(CmdIds.UnlockSkilltreeScRsp)
    {
        var proto = new UnlockSkilltreeScRsp
        {
            Retcode = (uint)retCode
        };

        SetData(proto);
    }

    public PacketUnlockSkilltreeScRsp(uint pointId, uint level) : base(CmdIds.UnlockSkilltreeScRsp)
    {
        var proto = new UnlockSkilltreeScRsp
        {
            PointId = pointId,
            Level = level
        };

        SetData(proto);
    }
}