using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Message;

public class PacketGetNpcStatusScRsp : BasePacket
{
    public PacketGetNpcStatusScRsp(PlayerInstance player) : base(CmdIds.GetNpcStatusScRsp)
    {
        var proto = new GetNpcStatusScRsp();

        foreach (var item in GameData.MessageContactsConfigData.Values)
            proto.NpcStatusList.Add(new NpcStatus
            {
                NpcId = (uint)item.ID,
                IsFinish = player.MessageManager!.GetMessageGroup(item.ID).Count > 0
            });

        SetData(proto);
    }
}