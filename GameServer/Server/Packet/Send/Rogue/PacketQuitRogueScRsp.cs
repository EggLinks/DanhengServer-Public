using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketQuitRogueScRsp : BasePacket
    {
        public PacketQuitRogueScRsp(PlayerInstance player) : base(CmdIds.QuitRogueScRsp)
        {
            var proto = new QuitRogueScRsp
            {
                RogueInfo = player.RogueManager!.ToProto(),
            };

            SetData(proto);
        }
    }
}
