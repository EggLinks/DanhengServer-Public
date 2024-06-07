using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.ChessRogue
{
    public class PacketChessRogueUpdateLevelBaseInfoScNotify : BasePacket
    {
        public PacketChessRogueUpdateLevelBaseInfoScNotify(ChessRogueLevelStatusType status) : base(CmdIds.ChessRogueUpdateLevelBaseInfoScNotify)
        {
            var proto = new ChessRogueUpdateLevelBaseInfoScNotify()
            {
                LevelStatus = status
            };

            SetData(proto);
        }
    }
}
