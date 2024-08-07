using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Tutorial;

public class PacketGetTutorialScRsp : BasePacket
{
    public PacketGetTutorialScRsp(PlayerInstance player) : base(CmdIds.GetTutorialScRsp)
    {
        var proto = new GetTutorialScRsp();

        foreach (var item in player.TutorialData!.Tutorials)
            proto.TutorialList.Add(new Proto.Tutorial
            {
                Id = (uint)item.Key,
                Status = item.Value
            });
        SetData(proto);
    }
}