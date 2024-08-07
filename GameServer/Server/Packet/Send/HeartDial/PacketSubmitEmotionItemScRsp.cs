using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;

public class PacketSubmitEmotionItemScRsp : BasePacket
{
    public PacketSubmitEmotionItemScRsp(uint scriptId) : base(CmdIds.SubmitEmotionItemScRsp)
    {
        var proto = new SubmitEmotionItemScRsp
        {
            ScriptId = scriptId
        };

        SetData(proto);
    }
}