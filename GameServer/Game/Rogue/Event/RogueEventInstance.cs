using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.Proto;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Game.Rogue.Event
{
    public class RogueEventInstance(int eventId, RogueNpc npc, List<RogueEventParam> optionIds, int uniqueId)
    {
        public int EventId { get; set; } = eventId;
        public bool Finished { get; set; } = false;
        public RogueNpc EventEntity { get; set; } = npc;
        public List<RogueEventParam> Options { get; set; } = optionIds;
        public int EventUniqueId { get; set; } = uniqueId;
        public List<int> SelectIds { get; set; } = [];

        public RogueEventInstance(RogueNPCDialogueExcel excel, RogueNpc npc, int uniqueId) : this(excel.RogueNPCID, npc, [], uniqueId)  // check in RogueInstance.cs
        {
            foreach (var option in excel.DialogueInfo!.DialogueIds)
            {
                GameData.DialogueEventData.TryGetValue(option, out var dialogueEvent);
                if (dialogueEvent == null) continue;

                var argId = 0;
                if (dialogueEvent.DynamicContentID > 0)
                {
                    GameData.DialogueDynamicContentData.TryGetValue(dialogueEvent.DynamicContentID, out var dynamicContent);
                    if (dynamicContent != null)
                    {
                        argId = dynamicContent.Keys.ToList().RandomElement();
                    }
                }

                Options.Add(new RogueEventParam()
                {
                    OptionId = option,
                    ArgId = argId,
                });
            }
        }

        public void Finish()
        {
            Finished = true;
            EventEntity.FinishDialogue();
        }

        public DialogueEvent ToProto()
        {
            var proto = new DialogueEvent()
            {
                EventId = (uint)EventId,
                GameModeType = (uint)EventEntity.Scene.Excel.PlaneType,
                EventUniqueId = (uint)EventUniqueId,
            };

            foreach (var option in Options)
            {
                proto.DialogueEventParamList.Add(option.ToProto());
            }

            proto.DialogueEventIdList.AddRange(SelectIds.Select(x => (uint)x));

            return proto;
        }
    }

    public class RogueEventParam
    {
        public int OptionId { get; set; }
        public int ArgId { get; set; }
        public float Ratio { get; set; }

        public RogueDialogueEventParam ToProto()
        {
            return new RogueDialogueEventParam()
            {
                DialogueEventId = (uint)OptionId,
                ArgId = (uint)ArgId,
                Ratio = Ratio,
                IsValid = true,
            };
        }

        public NpcDialogueEventParam ToNpcProto()
        {
            return new NpcDialogueEventParam()
            {
                DialogueEventId = (uint)OptionId,
                ArgId = (uint)ArgId,
            };
        }
    }
}
