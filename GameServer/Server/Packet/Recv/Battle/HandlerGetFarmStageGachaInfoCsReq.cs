using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Battle;

namespace EggLink.DanhengServer.Server.Packet.Recv.Battle
{
    [Opcode(CmdIds.GetFarmStageGachaInfoCsReq)]
    public class HandlerGetFarmStageGachaInfoCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = GetFarmStageGachaInfoCsReq.Parser.ParseFrom(data);
            connection.SendPacket(new PacketGetFarmStageGachaInfoScRsp(req));
        }
    }
}
