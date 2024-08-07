using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketPlayerGetTokenScRsp : BasePacket
{
    public PacketPlayerGetTokenScRsp(Connection connection) : base(CmdIds.PlayerGetTokenScRsp)
    {
        var rsp = new PlayerGetTokenScRsp
        {
            BlackInfo = new BlackInfo(),
            Uid = (uint)(connection.Player?.Uid ?? 0)
        };

        SetData(rsp);
    }

    public PacketPlayerGetTokenScRsp() : base(CmdIds.PlayerGetTokenScRsp)
    {
        var rsp = new PlayerGetTokenScRsp
        {
            Retcode = 0
        };

        SetData(rsp);
    }
}