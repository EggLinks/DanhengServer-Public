using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketStartCocoonStageScRsp : BasePacket
    {
        public PacketStartCocoonStageScRsp() : base(CmdIds.StartCocoonStageScRsp)
        {
            var rsp = new StartCocoonStageScRsp()
            {
                Retcode = 1
            };

            SetData(rsp);
        }

        public PacketStartCocoonStageScRsp(BattleInstance battle, int cocoonId, int wave) : base(CmdIds.StartCocoonStageScRsp)
        {
            var rsp = new StartCocoonStageScRsp()
            {
                CocoonId = (uint)cocoonId,
                Wave = (uint)wave,
                BattleInfo = battle.ToProto()
            };

            SetData(rsp);
        }
    }
}
