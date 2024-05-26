using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.Server.Packet.Send.Others
{
    public class PacketGetSecretKeyInfoScRsp : BasePacket
    {
        public PacketGetSecretKeyInfoScRsp() : base(CmdIds.GetSecretKeyInfoScRsp)
        {
            var proto = new GetSecretKeyInfoScRsp();
            proto.SecretInfo.Add(new SecretKeyInfo()
            {
                Type = SecretKeyType.SecretKeyVideo,
                Key = "10120425825329403",
            });

            SetData(proto);
        }
    }
}
