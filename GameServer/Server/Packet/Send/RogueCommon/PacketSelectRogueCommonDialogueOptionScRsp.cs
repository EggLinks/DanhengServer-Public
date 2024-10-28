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

        if (rogueEvent.EffectEventId.Count > 0)
        {
            proto.EffectEventIdList.AddRange(rogueEvent.EffectEventId.Select(x => (uint)x));
            rogueEvent.EffectEventId.Clear();
        }

        foreach (var option in rogueEvent.Options)
            if (option.OverrideSelected ?? option.IsSelected)
            {
                proto.EventHasEffect = true;
                break;
            }

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