using EggLink.DanhengServer.Data.Config.Scene;
using EggLink.DanhengServer.GameServer.Game.Rogue.Event;
using EggLink.DanhengServer.GameServer.Game.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Proto;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;

public class RogueNpc(SceneInstance scene, GroupInfo group, NpcInfo npcInfo) : EntityNpc(scene, group, npcInfo)
{
    public int RogueNpcId { get; set; }
    public int UniqueId { get; set; }
    private bool IsFinish { get; set; }

    public RogueEventInstance? RogueEvent { get; set; }

    public async ValueTask FinishDialogue()
    {
        IsFinish = true;
        await Scene.Player.SendPacket(new PacketSceneGroupRefreshScNotify(this));
    }

    public override SceneEntityInfo ToProto()
    {
        var proto = base.ToProto();

        if (RogueNpcId <= 0 || RogueEvent == null) return proto;
        proto.Npc.ExtraInfo = new NpcExtraInfo
        {
            RogueGameInfo = new NpcRogueGameInfo
            {
                TalkDialogueId = (uint)RogueNpcId,
                EventUniqueId = (uint)UniqueId,
                FinishDialogue = IsFinish
                //DialogueGroupId = (uint)GroupID
            }
        };

        foreach (var param in RogueEvent.Options)
            proto.Npc.ExtraInfo.RogueGameInfo.DialogueEventParamList.Add(param.ToNpcProto());

        return proto;
    }
}