using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Mission;
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
    public class PacketStoryLineInfoScNotify : BasePacket
    {
        public PacketStoryLineInfoScNotify(PlayerInstance player) : base(CmdIds.StoryLineInfoScNotify)
        {
            var storyLineIdList = player.StoryLineManager?.StoryLineData.RunningStoryLines.Keys.Select(x => (uint)x).ToList();
            storyLineIdList?.Insert(0, 0);

            var proto = new StoryLineInfoScNotify
            {
                CurStoryLineId = (uint)(player.StoryLineManager?.StoryLineData.CurStoryLineId ?? 0),
                RunningStoryLineIdList = { storyLineIdList },
            };

            GameData.StroyLineTrialAvatarDataData.TryGetValue(player.StoryLineManager!.StoryLineData.CurStoryLineId, out var storyExcel);
            if (storyExcel != null)
            {
                proto.TrialAvatarIdList.AddRange(storyExcel.InitTrialAvatarList.Select(x => (uint) x));
            }

            SetData(proto);
        }
    }
}
