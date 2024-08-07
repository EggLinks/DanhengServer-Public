using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.StoryLine;

public class PacketChangeStoryLineFinishScNotify : BasePacket
{
    public PacketChangeStoryLineFinishScNotify(int curId, ChangeStoryLineAction reason) : base(
        CmdIds.ChangeStoryLineFinishScNotify)
    {
        var proto = new ChangeStoryLineFinishScNotify
        {
            Action = reason,
            CurStoryLineId = (uint)curId
        };

        SetData(proto);
    }
}