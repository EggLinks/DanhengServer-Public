using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketEnterRogueMapRoomScRsp : BasePacket
    {
        public PacketEnterRogueMapRoomScRsp(PlayerInstance player) : base(CmdIds.EnterRogueMapRoomScRsp)
        {
            var proto = new EnterRogueMapRoomScRsp
            {
                CurSiteId = (uint)(player.RogueManager?.RogueInstance?.CurRoom?.SiteId ?? 0),
                Lineup = player.LineupManager!.GetCurLineup()!.ToProto(),
                Scene = player.SceneInstance!.ToProto(),
            };

            SetData(proto);
        }
    }
}
