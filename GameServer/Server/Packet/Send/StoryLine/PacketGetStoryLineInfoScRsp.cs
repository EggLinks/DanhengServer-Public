using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.StoryLine;

public class PacketGetStoryLineInfoScRsp : BasePacket
{
    public PacketGetStoryLineInfoScRsp(PlayerInstance player) : base(CmdIds.GetStoryLineInfoScRsp)
    {
        var proto = new GetStoryLineInfoScRsp
        {
            CurStoryLineId = (uint)player.StoryLineManager!.StoryLineData.CurStoryLineId,
            UnfinishedStoryLineIdList =
                { player.StoryLineManager!.StoryLineData.RunningStoryLines.Keys.Select(x => (uint)x) }
        };

        SetData(proto);
    }
}