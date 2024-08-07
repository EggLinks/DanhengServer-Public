using EggLink.DanhengServer.GameServer.Game.Rogue;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.Rogue;

public class PacketGetRogueTalentInfoScRsp : BasePacket
{
    public PacketGetRogueTalentInfoScRsp() : base(CmdIds.GetRogueTalentInfoScRsp)
    {
        var proto = new GetRogueTalentInfoScRsp
        {
            TalentInfoList = RogueManager.ToTalentProto()
        };

        SetData(proto);
    }
}