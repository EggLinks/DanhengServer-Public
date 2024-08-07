using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.Kcp;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

public class PacketSelectRogueCommonDialogueOptionScRsp : BasePacket
{
    public PacketSelectRogueCommonDialogueOptionScRsp(RogueEventInstance rogueEvent) : base(
        CmdIds.SelectRogueCommonDialogueOptionScRsp)
    {
        var proto = new SelectRogueCommonDialogueOptionScRsp
        {
            EventUniqueId = (uint)rogueEvent.EventUniqueId,
            DialogueData = rogueEvent.ToProto(),
            OptionId = (uint)rogueEvent.SelectedOptionId
        };

        SetData(proto);
    }

    public PacketSelectRogueCommonDialogueOptionScRsp() : base(CmdIds.SelectRogueCommonDialogueOptionScRsp)
    {
        var proto = new SelectRogueCommonDialogueOptionScRsp
        {
            Retcode = 1
        };

        SetData(proto);
    }
}