using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Activity;

public class PacketTrialActivityDataChangeScNotify : BasePacket
{
    public PacketTrialActivityDataChangeScNotify(uint stageId) : base(CmdIds.TrialActivityDataChangeScNotify)
    {
        var proto = new TrialActivityDataChangeScNotify
        {
            TrialActivityInfo =
            {
                StageId = stageId
            }
        };

        SetData(proto);
    }
}