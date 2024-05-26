using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
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
}
