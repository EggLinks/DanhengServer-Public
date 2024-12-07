using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;

public class PacketMusicRhythmUnlockSongSfxScNotify : BasePacket
{
    public PacketMusicRhythmUnlockSongSfxScNotify() : base(CmdIds.MusicRhythmUnlockSongSfxScNotify)
    {
        var proto = new MusicRhythmUnlockSongSfxScNotify();

        foreach (var sfx in GameData.MusicRhythmSoundEffectData.Values) proto.MusicUnlockList.Add((uint)sfx.GetId());

        SetData(proto);
    }
}