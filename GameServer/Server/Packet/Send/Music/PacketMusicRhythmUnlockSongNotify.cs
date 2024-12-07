using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;

public class PacketMusicRhythmUnlockSongNotify : BasePacket
{
    public PacketMusicRhythmUnlockSongNotify() : base(CmdIds.MusicRhythmUnlockSongNotify)
    {
        var proto = new MusicRhythmUnlockSongNotify();

        foreach (var song in GameData.MusicRhythmSongData.Values) proto.MusicUnlockList.Add((uint)song.GetId());

        SetData(proto);
    }
}