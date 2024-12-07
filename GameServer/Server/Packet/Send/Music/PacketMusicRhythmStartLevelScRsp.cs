using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;

public class PacketMusicRhythmStartLevelScRsp : BasePacket
{
    public PacketMusicRhythmStartLevelScRsp(uint levelId) : base(CmdIds.MusicRhythmStartLevelScRsp)
    {
        var proto = new MusicRhythmStartLevelScRsp
        {
            LevelId = levelId
        };

        SetData(proto);
    }
}