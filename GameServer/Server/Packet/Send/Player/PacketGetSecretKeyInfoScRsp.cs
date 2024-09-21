using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketGetSecretKeyInfoScRsp : BasePacket
{
    public PacketGetSecretKeyInfoScRsp() : base(CmdIds.GetSecretKeyInfoScRsp)
    {
        var proto = new GetSecretKeyInfoScRsp();
        proto.SecretInfo.Add(new SecretKeyInfo
        {
            Type = SecretKeyType.SecretKeyVideo,
            SecretKey = "10120425825329403"
        });

        SetData(proto);
    }
}