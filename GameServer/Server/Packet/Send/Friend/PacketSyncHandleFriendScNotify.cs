using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;

public class PacketSyncHandleFriendScNotify : BasePacket
{
    public PacketSyncHandleFriendScNotify(uint uid, bool isAccept, PlayerData playerData) : base(
        CmdIds.SyncHandleFriendScNotify)
    {
        var status = Listener.GetActiveConnection((int)uid) == null
            ? FriendOnlineStatus.Offline
            : FriendOnlineStatus.Online;
        var proto = new SyncHandleFriendScNotify
        {
            Uid = uid,
            IsAccept = isAccept,
            FriendInfo = new FriendSimpleInfo
            {
                IsMarked = false,
                RemarkName = "",
                PlayerInfo = playerData.ToSimpleProto(status)
            }
        };

        SetData(proto);
    }
}