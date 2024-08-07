using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.HeartDial;

public class PacketChangeScriptEmotionScRsp : BasePacket
{
    public PacketChangeScriptEmotionScRsp(uint scriptId, HeartDialEmotionType emotion) : base(
        CmdIds.ChangeScriptEmotionScRsp)
    {
        var proto = new ChangeScriptEmotionScRsp
        {
            ScriptId = scriptId,
            EmotionType = emotion
        };

        SetData(proto);
    }
}