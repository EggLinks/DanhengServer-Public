using EggLink.DanhengServer.Database.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Friend;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Friend;

[Opcode(CmdIds.GetPlayerDetailInfoCsReq)]
public class HandlerGetPlayerDetailInfoCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = GetPlayerDetailInfoCsReq.Parser.ParseFrom(data);

        var playerData = PlayerData.GetPlayerByUid(req.Uid);

        if (playerData == null)
        {
            var serverProfile = ConfigManager.Config.ServerOption.ServerProfile;
            if (req.Uid == serverProfile.Uid)
            {
                playerData = new PlayerData
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
                await connection.SendPacket(new PacketGetPlayerDetailInfoScRsp());
                return;
            }
        }

        await connection.SendPacket(new PacketGetPlayerDetailInfoScRsp(playerData));
    }
}