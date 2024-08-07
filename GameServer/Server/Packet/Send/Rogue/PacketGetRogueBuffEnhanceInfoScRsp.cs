using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketGetRogueBuffEnhanceInfoScRsp : BasePacket
{
    public PacketGetRogueBuffEnhanceInfoScRsp(PlayerInstance player) : base(CmdIds.GetRogueBuffEnhanceInfoScRsp)
    {
        var proto = new GetRogueBuffEnhanceInfoScRsp();
        if (player.RogueManager!.GetRogueInstance() == null)
        {
            proto.Retcode = 1;
            SetData(proto);
            return;
        }

        proto.BuffEnhanceInfo = player.RogueManager.GetRogueInstance()!.ToEnhanceInfo();

        SetData(proto);
    }
}