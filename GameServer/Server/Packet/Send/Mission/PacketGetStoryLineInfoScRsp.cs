using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Mission
{
    public class PacketGetStoryLineInfoScRsp : BasePacket
    {
        public PacketGetStoryLineInfoScRsp(PlayerInstance player) : base(CmdIds.GetStoryLineInfoScRsp)
        {
            var proto = new GetStoryLineInfoScRsp
            {
                CurStoryLineId = (uint)player.StoryLineManager!.StoryLineData.CurStoryLineId,
                RunningStoryLineIdList = { player.StoryLineManager!.StoryLineData.RunningStoryLines.Keys.Select(x => (uint)x) }
            };

            SetData(proto);
        }
    }
}
