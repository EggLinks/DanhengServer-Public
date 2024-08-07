using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.SetAvatarPathCsReq)]
public class HandlerSetAvatarPathCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SetAvatarPathCsReq.Parser.ParseFrom(data);

        GameData.MultiplePathAvatarConfigData.TryGetValue((int)req.AvatarId, out var avatar);

        if (avatar != null)
        {
            if (avatar.BaseAvatarID == 8001)
                await connection.Player!.ChangeAvatarPathType(avatar.BaseAvatarID,
                    (MultiPathAvatarTypeEnum)(avatar.AvatarID - (connection.Player.Data.CurrentGender - 1)));
            else
                await connection.Player!.ChangeAvatarPathType(avatar.BaseAvatarID,
                    (MultiPathAvatarTypeEnum)avatar.AvatarID);
            await connection.SendPacket(new PacketSetAvatarPathScRsp(avatar.AvatarID));
        }
        else
        {
            await connection.SendPacket(CmdIds.SetAvatarPathScRsp);
        }
    }
}