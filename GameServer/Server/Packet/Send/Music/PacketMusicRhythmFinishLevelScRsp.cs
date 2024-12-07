using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;

public class PacketMusicRhythmFinishLevelScRsp : BasePacket
{
    public PacketMusicRhythmFinishLevelScRsp(uint curLevel) : base(CmdIds.MusicRhythmFinishLevelScRsp)
    {
        var proto = new MusicRhythmFinishLevelScRsp
        {
            LevelId = curLevel
        };

        SetData(proto);
    }
}