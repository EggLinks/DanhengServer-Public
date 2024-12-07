using EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;
using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Music;

[Opcode(CmdIds.MusicRhythmDataCsReq)]
public class HandlerMusicRhythmDataCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(new PacketMusicRhythmDataScRsp());

        // Unlock max difficulty level
        await connection.SendPacket(CmdIds.MusicRhythmMaxDifficultyLevelsUnlockNotify);

        // Unknwon fields
        await connection.SendPacket(new PacketMusicRhythmUnlockSongNotify());
        await connection.SendPacket(new PacketMusicRhythmUnlockSongSfxScNotify());
        await connection.SendPacket(new PacketMusicRhythmUnlockTrackScNotify());
    }
}