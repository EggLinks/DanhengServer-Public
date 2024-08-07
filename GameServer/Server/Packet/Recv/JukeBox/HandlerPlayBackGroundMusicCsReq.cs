using EggLink.DanhengServer.GameServer.Server.Packet.Send.JukeBox;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.JukeBox;

[Opcode(CmdIds.PlayBackGroundMusicCsReq)]
public class HandlerPlayBackGroundMusicCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = PlayBackGroundMusicCsReq.Parser.ParseFrom(data);

        connection.Player!.Data.CurrentBgm = (int)req.PlayMusicId;

        await connection.SendPacket(new PacketPlayBackGroundMusicScRsp(req.PlayMusicId));
    }
}