using EggLink.DanhengServer.Server.Packet.Send.Tutorial;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Server.Packet.Recv.Tutorial
{
    [Opcode(CmdIds.GetTutorialGuideCsReq)]
    public class HandlerGetTutorialGuideCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {

            if (ConfigManager.Config.ServerOption.EnableMission)  // If missions are enabled
                connection.SendPacket(new PacketGetTutorialGuideScRsp(connection.Player!));  // some bug
        }
    }
}
