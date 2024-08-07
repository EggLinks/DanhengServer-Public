using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Player;

public class PacketSetGameplayBirthdayScRsp : BasePacket
{
    public PacketSetGameplayBirthdayScRsp(uint birthday) : base(CmdIds.SetGameplayBirthdayScRsp)
    {
        var proto = new SetGameplayBirthdayScRsp
        {
            Birthday = birthday
        };

        SetData(proto);
    }

    public PacketSetGameplayBirthdayScRsp() : base(CmdIds.SetGameplayBirthdayScRsp)
    {
        var proto = new SetGameplayBirthdayScRsp
        {
            Retcode = 1
        };

        SetData(proto);
    }
}