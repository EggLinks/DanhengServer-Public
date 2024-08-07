using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketSetPlayerInfoScRsp : BasePacket
{
    public PacketSetPlayerInfoScRsp(PlayerInstance player, bool IsModify) : base(CmdIds.SetPlayerInfoScRsp)
    {
        var proto = new SetPlayerInfoScRsp
        {
            CurAvatarPath = (MultiPathAvatarType)player.Data.CurBasicType,
            IsModify = IsModify
        };

        SetData(proto);
    }
}