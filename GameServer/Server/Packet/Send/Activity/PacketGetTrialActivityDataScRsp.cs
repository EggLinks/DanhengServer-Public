using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Activity;

public class PacketGetTrialActivityDataScRsp : BasePacket
{
    public PacketGetTrialActivityDataScRsp(PlayerInstance player) : base(CmdIds.GetTrialActivityDataScRsp)
    {
        var proto = new GetTrialActivityDataScRsp();
        proto.TrialActivityInfoList.Add(player.ActivityManager!.Data.TrialActivityData.ToProto());
        SetData(proto);
    }
}