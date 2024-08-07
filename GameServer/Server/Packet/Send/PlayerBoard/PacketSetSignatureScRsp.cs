using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;

public class PacketSetSignatureScRsp : BasePacket
{
    public PacketSetSignatureScRsp(string signature) : base(CmdIds.SetSignatureScRsp)
    {
        var proto = new SetSignatureScRsp
        {
            Signature = signature
        };

        SetData(proto);
    }
}