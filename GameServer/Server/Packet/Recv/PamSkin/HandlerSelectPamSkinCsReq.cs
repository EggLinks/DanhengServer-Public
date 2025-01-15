using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PamSkin;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.PamSkin;

[Opcode(CmdIds.SelectPamSkinCsReq)]
public class HandlerSelectPamSkinCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        var req = SelectPamSkinCsReq.Parser.ParseFrom(data);
        var player = connection.Player!;

        // Check if the skin is valid
        if (GameData.PamSkinConfigData.ContainsKey((int)req.PamSkinId)) player.Data.CurrentPamSkin = (int)req.PamSkinId;
        var prevSkinId = player.Data.CurrentPamSkin;

        await connection.SendPacket(new PacketSelectPamSkinScRsp(player, prevSkinId));
    }
}