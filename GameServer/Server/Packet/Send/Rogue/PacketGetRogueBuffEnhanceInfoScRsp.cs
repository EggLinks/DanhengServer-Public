using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketGetRogueBuffEnhanceInfoScRsp : BasePacket
    {
        public PacketGetRogueBuffEnhanceInfoScRsp(PlayerInstance player) : base(CmdIds.GetRogueBuffEnhanceInfoScRsp)
        {
            var proto = new GetRogueBuffEnhanceInfoScRsp();
            if (player.RogueManager!.GetRogueInstance() == null)
            {
                proto.Retcode = 1;
                SetData(proto);
                return;
            }
            proto.BuffEnhanceInfo = player.RogueManager.GetRogueInstance()!.ToEnhanceInfo();

            SetData(proto);
        }
    }
}
