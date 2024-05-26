using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketAcceptMainMissionScRsp : BasePacket
    {
        public PacketAcceptMainMissionScRsp(uint missionId) : base(CmdIds.AcceptMainMissionScRsp)
        {
            var proto = new AcceptMainMissionScRsp()
            {
                MainMissionId = missionId,
            };

            SetData(proto);
        }
    }
}
