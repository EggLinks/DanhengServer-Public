using EggLink.DanhengServer.Game.Rogue.Buff;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Rogue
{
    public class PacketHandleRogueCommonPendingActionScRsp : BasePacket
    {
        public PacketHandleRogueCommonPendingActionScRsp(int queuePosition, bool selectBuff = false, bool selectMiracle = false, bool selectBonus = false, RogueBuffSelectMenu? menu = null) : base(CmdIds.HandleRogueCommonPendingActionScRsp)
        {
            var proto = new HandleRogueCommonPendingActionScRsp
            {
                QueueLocation = (uint)queuePosition,
                QueuePosition = (uint)queuePosition,
            };

            if (selectBuff)
            {
                proto.BuffSelectCallback = new();
            }

            if (selectMiracle)
            {
                proto.MiracleSelectCallback = new();
            }

            if (selectBonus)
            {
                proto.BonusSelectCallback = new();
            }

            if (menu != null)
            {
                proto.BuffRerollCallback = new()
                {
                    BuffSelectInfo = menu.ToProto()
                };
            }

            SetData(proto);
        }
    }
}
