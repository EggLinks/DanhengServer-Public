using EggLink.DanhengServer.GameServer.Server.Packet.Send.Music;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Music;

[Opcode(CmdIds.MusicRhythmStartLevelCsReq)]
public class HandlerMusicRhythmStartLevelCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = MusicRhythmStartLevelCsReq.Parser.ParseFrom(data);
        var curLevel = req.LevelId;

        connection.Player!.Data.CurMusicLevel = (int)curLevel;

        await connection.SendPacket(new PacketMusicRhythmStartLevelScRsp(curLevel));
    }
}