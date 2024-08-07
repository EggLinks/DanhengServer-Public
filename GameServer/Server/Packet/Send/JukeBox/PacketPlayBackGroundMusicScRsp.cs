using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.JukeBox;

public class PacketPlayBackGroundMusicScRsp : BasePacket
{
    public PacketPlayBackGroundMusicScRsp(uint musicId) : base(CmdIds.PlayBackGroundMusicScRsp)
    {
        var proto = new PlayBackGroundMusicScRsp
        {
            PlayMusicId = musicId,
            CurrentMusicId = musicId
        };

        SetData(proto);
    }
}