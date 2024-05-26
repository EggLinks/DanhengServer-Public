using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Game.Rogue;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketGetRogueTalentInfoScRsp : BasePacket
    {
        public PacketGetRogueTalentInfoScRsp() : base(CmdIds.GetRogueTalentInfoScRsp)
        {
            var proto = new GetRogueTalentInfoScRsp()
            {
                RogueTalentInfo = RogueManager.ToTalentProto()
            };

            SetData(proto);
        }
    }
}
