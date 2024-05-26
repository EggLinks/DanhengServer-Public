using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Proto;
using Google.Protobuf.Collections;

namespace EggLink.DanhengServer.Server.Packet.Send.Avatar
{
    public class PacketSetAssistAvatarScRsp : BasePacket
    {
        public PacketSetAssistAvatarScRsp(RepeatedField<uint> avatarId) : base(CmdIds.SetAssistAvatarScRsp)
        {
            var proto = new SetAssistAvatarScRsp();
            proto.AvatarIdList.AddRange(avatarId);

            SetData(proto);
        }
    }
}
