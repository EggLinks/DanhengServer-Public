using EggLink.DanhengServer.Database;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Server.Packet.Send.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Server.Packet.Recv.Player
{
    [Opcode(CmdIds.SetSignatureCsReq)]
    public class HandlerSetSignatureCsReq : Handler
    {
        public override void OnHandle(Connection connection, byte[] header, byte[] data)
        {
            var req = SetSignatureCsReq.Parser.ParseFrom(data);

            connection.Player!.Data.Signature = req.Signature;
            DatabaseHelper.Instance!.UpdateInstance(connection.Player!.Data);

            connection.SendPacket(new PacketSetSignatureScRsp(req.Signature));
        }
    }
}
