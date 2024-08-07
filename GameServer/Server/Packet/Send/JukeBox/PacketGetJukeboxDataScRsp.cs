using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.JukeBox;

public class PacketGetJukeboxDataScRsp : BasePacket
{
    public PacketGetJukeboxDataScRsp(PlayerInstance player) : base(CmdIds.GetJukeboxDataScRsp)
    {
        var proto = new GetJukeboxDataScRsp
        {
            CurrentMusicId = (uint)player.Data.CurrentBgm
        };

        foreach (var music in GameData.BackGroundMusicData.Values)
            proto.UnlockedMusicList.Add(new MusicData
            {
                Id = (uint)music.ID,
                GroupId = (uint)music.GroupID,
                IsPlayed = true
            });

        SetData(proto);
    }
}