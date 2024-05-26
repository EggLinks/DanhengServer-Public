using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketSceneCastSkillScRsp : BasePacket
    {
        public PacketSceneCastSkillScRsp(uint castEntityId) : base(CmdIds.SceneCastSkillScRsp)
        {
            var proto = new SceneCastSkillScRsp()
            {
                CastEntityId = castEntityId,
            };

            SetData(proto);
        }

        public PacketSceneCastSkillScRsp(uint castEntityId, BattleInstance battle) : base(CmdIds.SceneCastSkillScRsp)
        {
            var proto = new SceneCastSkillScRsp()
            {
                CastEntityId = castEntityId,
                BattleInfo = battle.ToProto(),
            };

            SetData(proto);
        }
    }
}
