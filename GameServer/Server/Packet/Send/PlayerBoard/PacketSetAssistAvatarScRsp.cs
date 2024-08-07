using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using Google.Protobuf.Collections;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;

public class PacketSetAssistAvatarScRsp : BasePacket
{
    public PacketSetAssistAvatarScRsp(RepeatedField<uint> avatarId) : base(CmdIds.SetAssistAvatarScRsp)
    {
        var proto = new SetAssistAvatarScRsp();
        proto.AvatarIdList.AddRange(avatarId);

        SetData(proto);
    }
}