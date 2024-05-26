using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Friend
{
    [Opcode(CmdIds.GetPlayerDetailInfoCsReq)]
    public class HandlerGetPlayerDetailInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetPlayerDetailInfoCsReq.Parser.ParseFrom(data);

            var playerData = PlayerData.GetPlayerByUid(req.Uid);

            if (playerData == null )
            {
                var serverProfile = ConfigManager.Config.ServerOption.ServerProfile;
                if (req.Uid == serverProfile.Uid)
                {
                    playerData = new()
                    {
                        Uid = serverProfile.Uid,
                        HeadIcon = serverProfile.HeadIcon,
                        Signature = serverProfile.Signature,
                        Level = serverProfile.Level,
                        WorldLevel = 0,
                        Name = serverProfile.Name,
                        ChatBubble = serverProfile.ChatBubbleId
                    };
                }
                else
                {
                    connection.SendPacket(new PacketGetPlayerDetailInfoScRsp());
                    return;
                }
            }

            connection.SendPacket(new PacketGetPlayerDetailInfoScRsp(playerData));
        }
    }
}
