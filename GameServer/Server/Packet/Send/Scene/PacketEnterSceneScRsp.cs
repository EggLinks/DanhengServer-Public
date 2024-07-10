using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene
{
    public class PacketEnterSceneScRsp : BasePacket
    {
        public PacketEnterSceneScRsp(bool overMapTp = false, bool tpByMap = false, int storyLineId = 0) : base(CmdIds.EnterSceneScRsp)
        {
            var proto = new EnterSceneScRsp
            {
                GameStoryLineId = (uint)storyLineId,
                MapTp = tpByMap,
                JDALDJPFNMN = overMapTp
            };

            SetData(proto);
        }
    }
}
