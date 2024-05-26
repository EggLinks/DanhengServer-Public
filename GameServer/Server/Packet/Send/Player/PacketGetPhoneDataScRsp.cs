using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Send.Player
{
    public class PacketGetPhoneDataScRsp : BasePacket
    {
        public PacketGetPhoneDataScRsp(PlayerInstance player) : base(CmdIds.GetPhoneDataScRsp)
        {
            var proto = new GetPhoneDataScRsp
            {
                CurChatBubble = (uint)player.Data.ChatBubble,
                CurPhoneTheme = (uint)player.Data.PhoneTheme,
            };

            foreach (var item in player.PlayerUnlockData!.PhoneThemes)
            {
                proto.OwnedPhoneThemes.Add((uint)item);
            }

            foreach (var item in player.PlayerUnlockData!.ChatBubbles)
            {
                proto.OwnedChatBubbles.Add((uint)item);
            }

            SetData(proto);
        }
    }
}
