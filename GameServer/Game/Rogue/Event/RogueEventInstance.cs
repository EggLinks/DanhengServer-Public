using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.GameServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event;

public class RogueEventInstance(int eventId, RogueNpc npc, List<RogueEventParam> optionIds, int uniqueId)
{
    public RogueEventInstance(RogueNPCExcel excel, RogueNpc npc, int uniqueId) : this(excel.RogueNPCID, npc, [],
        uniqueId) // check in RogueInstance.cs
    {
        foreach (var option in excel.RogueNpcConfig!.DialogueList[0].OptionInfo?.OptionList ?? [])
        {
            GameData.DialogueEventData.TryGetValue(option.OptionID, out var dialogueEvent);
            if (dialogueEvent == null) continue;

            var argId = 0;
            if (dialogueEvent.DynamicContentID > 0)
            {
                GameData.DialogueDynamicContentData.TryGetValue(dialogueEvent.DynamicContentID, out var dynamicContent);
                if (dynamicContent != null) argId = dynamicContent.Keys.ToList().RandomElement();
            }

            Options.Add(new RogueEventParam
            {
                OptionId = option.OptionID,
                ArgId = argId
            });
        }
    }

    public int EventId { get; set; } = eventId;
    public bool Finished { get; set; }
    public RogueNpc EventEntity { get; set; } = npc;
    public List<RogueEventParam> Options { get; set; } = optionIds;
    public int EventUniqueId { get; set; } = uniqueId;
    public int SelectedOptionId { get; set; } = 0;
    public List<int> EffectEventId { get; set; } = [];

    public async ValueTask Finish()
    {
        Finished = true;
        await EventEntity.FinishDialogue();
    }

    public RogueCommonDialogueDataInfo ToProto()
    {
        var proto = new RogueCommonDialogueDataInfo
        {
            DialogueInfo = ToDialogueInfo(),
            EventUniqueId = (uint)EventUniqueId
        };

        foreach (var option in Options) proto.OptionList.Add(option.ToProto());

        return proto;
    }

    public RogueCommonDialogueInfo ToDialogueInfo()
    {
        var proto = new RogueCommonDialogueInfo
        {
            DialogueBasicInfo = new RogueCommonDialogueBasicInfo
            {
                TalkDialogueId = (uint)EventId
            }
        };

        return proto;
    }
}

public class RogueEventParam
{
    public int OptionId { get; set; }
    public int ArgId { get; set; }
    public float Ratio { get; set; }
    public bool IsSelected { get; set; } = false;
    public bool? OverrideSelected { get; set; } = null;
    public List<RogueEventResultInfo> Results { get; set; } = [];

    public RogueCommonDialogueOptionInfo ToProto()
    {
        return new RogueCommonDialogueOptionInfo
        {
            ArgId = (uint)ArgId,
            IsValid = true,
            OptionId = (uint)OptionId,
            DisplayValue = new RogueCommonDialogueOptionDisplayInfo
            {
                DisplayFloatValue = Ratio
            },
            OptionResultInfo = { Results.Select(x => x.ToProto()) },
            Confirm = OverrideSelected ?? IsSelected
        };
    }

    public NpcDialogueEventParam ToNpcProto()
    {
        return new NpcDialogueEventParam
        {
            RogueDialogueEventId = (uint)OptionId,
            ArgId = (uint)ArgId
        };
    }
}

public class RogueEventResultInfo
{
    public int BattleEventId { get; set; }

    public RogueCommonDialogueOptionResultInfo ToProto()
    {
        return new RogueCommonDialogueOptionResultInfo
        {
            BattleResultInfo = new RogueCommonDialogueOptionBattleResultInfo
            {
                BattleEventId = (uint)BattleEventId
            }
        };
    }
}