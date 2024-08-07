using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketSyncApplyFriendScNotify : BasePacket
{
    public PacketSyncApplyFriendScNotify(PlayerData player) : base(CmdIds.SyncApplyFriendScNotify)
    {
        var proto = new SyncApplyFriendScNotify
        {
            ApplyInfo = new FriendApplyInfo
            {
                ApplyTime = Extensions.GetUnixSec(),
                PlayerInfo = player.ToSimpleProto(FriendOnlineStatus.Online)
            }
        };

        SetData(proto);
    }
}