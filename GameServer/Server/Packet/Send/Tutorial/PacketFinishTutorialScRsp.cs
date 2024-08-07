using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Tutorial;

public class PacketFinishTutorialScRsp : BasePacket
{
    public PacketFinishTutorialScRsp(uint tutorialId) : base(CmdIds.FinishTutorialScRsp)
    {
        var rsp = new FinishTutorialScRsp
        {
            Tutorial = new Proto.Tutorial
            {
                Id = tutorialId,
                Status = TutorialStatus.TutorialFinish
            }
        };

        SetData(rsp);
    }
}