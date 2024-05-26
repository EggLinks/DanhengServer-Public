using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Friend
{
    public class PacketSyncApplyFriendScNotify : BasePacket
    {
        public PacketSyncApplyFriendScNotify(PlayerData player) : base(CmdIds.SyncApplyFriendScNotify)
        {
            var proto = new SyncApplyFriendScNotify()
            {
                ApplyInfo = new FriendApplyInfo()
                {
                    ApplyTime = Extensions.GetUnixSec(),
                    PlayerInfo = player.ToSimpleProto(FriendOnlineStatus.Online),
                },
            };

            SetData(proto);
        }
    }
}
