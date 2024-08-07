using EggLink.DanhengServer.GameServer.Server.Packet.Send.Tutorial;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Tutorial;

[Opcode(CmdIds.GetTutorialGuideCsReq)]
public class HandlerGetTutorialGuideCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        if (ConfigManager.Config.ServerOption.EnableMission) // If missions are enabled
            await connection.SendPacket(new PacketGetTutorialGuideScRsp(connection.Player!)); // some bug
    }
}