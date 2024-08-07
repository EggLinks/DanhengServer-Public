using EggLink.DanhengServer.Kcp;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Recv.Player;

[Opcode(CmdIds.PlayerLoginFinishCsReq)]
public class HandlerPlayerLoginFinishCsReq : Handler
{
    public override async Task OnHandle(Connection connection, byte[] header, byte[] data)
    {
        await connection.SendPacket(CmdIds.PlayerLoginFinishScRsp);
        //var list = connection.Player!.MissionManager!.GetRunningSubMissionIdList();
        //connection.SendPacket(new PacketMissionAcceptScNotify(list));
    }
}