using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Gacha
{
    public class PacketGetGachaInfoScRsp : BasePacket
    {
        public PacketGetGachaInfoScRsp(PlayerInstance player) : base(CmdIds.GetGachaInfoScRsp)
        {
            SetData(player.GachaManager!.ToProto());
        }
    }
}
