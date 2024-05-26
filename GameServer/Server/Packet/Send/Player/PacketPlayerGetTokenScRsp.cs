using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketPlayerGetTokenScRsp : BasePacket
    {
        public PacketPlayerGetTokenScRsp(Connection connection) : base(CmdIds.PlayerGetTokenScRsp)
        {
            var rsp = new PlayerGetTokenScRsp()
            {
                BlackInfo = new(),
                Uid = connection.Player?.Uid ?? 0,
            };

            SetData(rsp);
        }
        public PacketPlayerGetTokenScRsp() : base(CmdIds.PlayerGetTokenScRsp)
        {
            var rsp = new PlayerGetTokenScRsp()
            {
                Retcode = 0,
            };

            SetData(rsp);
        }
    }
}
