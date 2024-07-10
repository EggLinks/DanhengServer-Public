using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Battle
{
    public class PacketGetCurBattleInfoScRsp : BasePacket
    {
        public PacketGetCurBattleInfoScRsp() : base(CmdIds.GetCurBattleInfoScRsp)
        {
            var proto = new GetCurBattleInfoScRsp()
            {
                BattleInfo = new()
                {
                    BattleTargetInfo = {}
                },
                FFKDJNEAIOG = { },
            };

            SetData(proto);
        }
    }
}
