using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketSetClientPausedScRsp : BasePacket
    {
        public PacketSetClientPausedScRsp(bool paused) : base(CmdIds.SetClientPausedScRsp)
        {
            var rsp = new SetClientPausedScRsp
            {
                Paused = paused
            };
            SetData(rsp);
        }
    }
}
