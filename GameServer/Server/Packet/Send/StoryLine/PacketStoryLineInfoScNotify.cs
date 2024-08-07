using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.StoryLine;

public class PacketStoryLineInfoScNotify : BasePacket
{
    public PacketStoryLineInfoScNotify(PlayerInstance player) : base(CmdIds.StoryLineInfoScNotify)
    {
        var storyLineIdList =
            player.StoryLineManager?.StoryLineData.RunningStoryLines.Keys.Select(x => (uint)x).ToList() ?? [];
        storyLineIdList.Insert(0, 0);

        var proto = new StoryLineInfoScNotify
        {
            CurStoryLineId = (uint)(player.StoryLineManager?.StoryLineData.CurStoryLineId ?? 0),
            UnfinishedStoryLineIdList = { storyLineIdList }
        };

        if (!proto.UnfinishedStoryLineIdList.Contains(proto.CurStoryLineId))
            proto.UnfinishedStoryLineIdList.Add(proto.CurStoryLineId);

        GameData.StroyLineTrialAvatarDataData.TryGetValue(player.StoryLineManager!.StoryLineData.CurStoryLineId,
            out var storyExcel);
        if (storyExcel != null) proto.TrialAvatarIdList.AddRange(storyExcel.InitTrialAvatarList.Select(x => (uint)x));

        SetData(proto);
    }
}