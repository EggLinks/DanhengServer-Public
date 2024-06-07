using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketStartRogueScRsp : BasePacket
    {
        public PacketStartRogueScRsp(PlayerInstance player) : base(CmdIds.StartRogueScRsp)
        {
            var proto = new StartRogueScRsp
            {
                RogueInfo = player.RogueManager!.ToProto(),
                Lineup = player.LineupManager!.GetCurLineup()!.ToProto(),
                Scene = player.SceneInstance!.ToProto(),
            };

            SetData(proto);
        }
    }
}
