using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketGetChessRogueBuffEnhanceInfoScRsp : BasePacket
    {
        public PacketGetChessRogueBuffEnhanceInfoScRsp(PlayerInstance player) : base(CmdIds.GetChessRogueBuffEnhanceInfoScRsp)
        {
            var proto = new GetChessRogueBuffEnhanceInfoScRsp();
            if (player.ChessRogueManager!.RogueInstance == null)
            {
                proto.Retcode = 1;
                SetData(proto);
                return;
            }
            proto.BuffEnhanceInfo = player.ChessRogueManager.RogueInstance!.ToChessEnhanceInfo();

            SetData(proto);
        }
    }
}
