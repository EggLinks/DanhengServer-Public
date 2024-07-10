using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Server.Packet.Send.Mission;
using EggLink.DanhengServer.Server.Packet.Send.Others;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.PlayerLoginFinishCsReq)]
    public class HandlerPlayerLoginFinishCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(CmdIds.PlayerLoginFinishScRsp);
            //var list = connection.Player!.MissionManager!.GetRunningSubMissionIdList();
            //connection.SendPacket(new PacketMissionAcceptScNotify(list));
        }
    }
}
