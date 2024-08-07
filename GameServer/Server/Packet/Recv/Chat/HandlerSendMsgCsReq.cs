using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Chat;

[Opcode(CmdIds.SendMsgCsReq)]
public class HandlerSendMsgCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SendMsgCsReq.Parser.ParseFrom(data);

        await connection.SendPacket(CmdIds.SendMsgScRsp);

        if (req.MessageType == MsgType.CustomText)
            await connection.Player!.FriendManager!.SendMessage(connection.Player!.Uid, (int)req.TargetList[0],
                req.MessageText);
        else if (req.MessageType == MsgType.Emoji)
            await connection.Player!.FriendManager!.SendMessage(connection.Player!.Uid, (int)req.TargetList[0], null,
                (int)req.ExtraId);
    }
}