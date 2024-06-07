using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketLeaveRogueScRsp : BasePacket
    {
        public PacketLeaveRogueScRsp(PlayerInstance player) : base(CmdIds.LeaveRogueScRsp)
        {
            var proto = new LeaveRogueScRsp()
            {
                Lineup = player.LineupManager!.GetCurLineup()!.ToProto(),
                Scene = player.SceneInstance!.ToProto(),
                RogueInfo = player.RogueManager!.ToProto(),
            };

            SetData(proto);
        }
    }
}
