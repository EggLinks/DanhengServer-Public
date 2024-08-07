using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;
using Google.Protobuf.Collections;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerBoard;

public class PacketSetDisplayAvatarScRsp : BasePacket
{
    public PacketSetDisplayAvatarScRsp(RepeatedField<DisplayAvatarData> avatars) : base(CmdIds.SetDisplayAvatarScRsp)
    {
        var proto = new SetDisplayAvatarScRsp();
        proto.DisplayAvatarList.AddRange(avatars);

        SetData(proto);
    }
}