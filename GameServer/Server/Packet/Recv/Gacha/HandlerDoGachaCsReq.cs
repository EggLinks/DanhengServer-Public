using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Gacha;

namespace EggLink.DanhengServer.Server.Packet.Recv.Gacha
{
    [Opcode(CmdIds.DoGachaCsReq)]
    public class HandlerDoGachaCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = DoGachaCsReq.Parser.ParseFrom(data);
            var gain = connection.Player!.GachaManager?.DoGacha((int)req.GachaId, (int)req.GachaNum);

            if (gain != null)
            {
                connection.SendPacket(new PacketDoGachaScRsp(gain));
            } else
            {
                connection.SendPacket(new PacketDoGachaScRsp());
            }
        }
    }
}
