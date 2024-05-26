using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Mission
{
    public class PacketStartFinishSubMissionScNotify : BasePacket
    {
        public PacketStartFinishSubMissionScNotify(int missionId) : base(CmdIds.StartFinishSubMissionScNotify)
        {
            var proto = new StartFinishSubMissionScNotify()
            {
                SubMissionId = (uint)missionId,
            };

            SetData(proto);
        }
    }
}
