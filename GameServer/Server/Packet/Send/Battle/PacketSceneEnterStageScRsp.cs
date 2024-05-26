using EggLink.DanhengServer.Game.Battle;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Battle
{
    public class PacketSceneEnterStageScRsp : BasePacket
    {
        public PacketSceneEnterStageScRsp() : base(CmdIds.SceneEnterStageScRsp)
        {
            var proto = new SceneEnterStageScRsp()
            {
                Retcode = 1,
            };

            SetData(proto);
        }

        public PacketSceneEnterStageScRsp(BattleInstance battleInstance) : base(CmdIds.SceneEnterStageScRsp)
        {
            var proto = new SceneEnterStageScRsp()
            {
                BattleInfo = battleInstance.ToProto(),
            };

            SetData(proto);
        }
    }
}
