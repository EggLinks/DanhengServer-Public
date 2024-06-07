using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Friend
{
    public class PacketSyncHandleFriendScNotify : BasePacket
    {
        public PacketSyncHandleFriendScNotify(uint uid, bool isAccept, PlayerData playerData) : base(CmdIds.SyncHandleFriendScNotify)
        {
            var status = Listener.GetActiveConnection((int)uid) == null ? FriendOnlineStatus.Offline : FriendOnlineStatus.Online;
            var proto = new SyncHandleFriendScNotify
            {
                Uid = uid,
                IsAccept = isAccept,
                FriendInfo = new()
                {
                    IsMarked = false,
                    RemarkName = "",
                    PlayerInfo = playerData.ToSimpleProto(status)
                }
            };

            SetData(proto);
        }
    }
}
