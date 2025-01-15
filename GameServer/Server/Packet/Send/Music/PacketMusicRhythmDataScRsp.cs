using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;

public class PacketMusicRhythmDataScRsp : BasePacket
{
    public PacketMusicRhythmDataScRsp() : base(CmdIds.MusicRhythmDataScRsp)
    {
        var proto = new MusicRhythmDataScRsp
        {
            ShowHint = true
        };

        foreach (var level in GameData.MusicRhythmLevelData.Values)
            proto.MusicLevel.Add(new MusicRhythmLevel
            {
                LevelId = (uint)level.GetId(),
                FullCombo = true,
                UnlockLevel = 3
            });

        foreach (var group in GameData.MusicRhythmGroupData.Values)
            proto.MusicGroup.Add(new MusicRhythmGroup
            {
                MusicGroupId = (uint)group.GetId(),
                MusicGroupPhase = (uint)group.Phase
            });

        foreach (var song in GameData.MusicRhythmSongData.Values) proto.UnlockSongList.Add((uint)song.GetId());

        foreach (var track in GameData.MusicRhythmTrackData.Values) proto.UnlockTrackList.Add((uint)track.GetId());

        foreach (var phase in GameData.MusicRhythmPhaseData.Values) proto.UnlockPhaseList.Add((uint)phase.GetId());

        SetData(proto);
    }
}