using EggLink.DanhengServer.Server.Packet.Send.Mission;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.PlayerLoginFinishCsReq)]
    public class HandlerPlayerLoginFinishCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            connection.SendPacket(CmdIds.PlayerLoginFinishScRsp);
            connection.SendPacket(CmdIds.GetArchiveDataScRsp);
            var list = connection.Player!.MissionManager!.GetRunningSubMissionIdList();
            connection.SendPacket(new PacketMissionAcceptScNotify(list));
        }
    }
}
