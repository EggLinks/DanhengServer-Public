using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.ChessRogue;

public class PacketGetChessRogueNousStoryInfoScRsp : BasePacket
{
    public PacketGetChessRogueNousStoryInfoScRsp() : base(CmdIds.GetChessRogueNousStoryInfoScRsp)
    {
        var proto = new GetChessRogueNousStoryInfoScRsp();

        foreach (var item in GameData.RogueNousMainStoryData.Values)
            proto.ChessRogueMainStoryInfo.Add(new ChessRogueNousMainStoryInfo
            {
                ChessRogueMainStoryId = (uint)item.StoryID,
                Status = ChessRogueNousMainStoryStatus.Finish
            });

        foreach (var item in GameData.RogueNousSubStoryData.Values)
            proto.ChessRogueSubStoryInfo.Add(new ChessRogueNousSubStoryInfo
            {
                ChessRogueSubStoryId = (uint)item.StoryID
            });

        SetData(proto);
    }
}