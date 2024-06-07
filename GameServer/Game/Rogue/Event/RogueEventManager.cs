using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.Game.Player;
using EggLink.DanhengServer.Server.Packet.Send.Rogue;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Game.Rogue.Event
{
    public class RogueEventManager
    {
        public PlayerInstance Player;
        public BaseRogueInstance Rogue;
        public List<RogueEventInstance> RunningEvent = [];
        public Dictionary<DialogueEventTypeEnum, RogueEventEffectHandler> EffectHandler = [];
        public Dictionary<DialogueEventCostTypeEnum, RogueEventCostHandler> CostHandler = [];

        public RogueEventManager(PlayerInstance player, BaseRogueInstance rogueInstance)
        {
            Player = player;
            Rogue = rogueInstance;

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<RogueEventAttribute>();
                if (attr != null)
                {
                    if (attr.EffectType != DialogueEventTypeEnum.None)
                    {
                        // Effect
                        var effect = (RogueEventEffectHandler)Activator.CreateInstance(type, null)!;
                        EffectHandler.Add(attr.EffectType, effect);
                    } else
                    {
                        // Cost
                        var cost = (RogueEventCostHandler)Activator.CreateInstance(type, null)!;
                        CostHandler.Add(attr.CostType, cost);
                    }
                }
            }
        }

        public void OnNextRoom()
        {
            RunningEvent.Clear();  // Clear all running events
        }

        public void AddEvent(RogueEventInstance eventInstance)
        {
            RunningEvent.Add(eventInstance);
            Player.SendPacket(new PacketSyncRogueDialogueEventDataScNotify(eventInstance));
        }

        public void RemoveEvent(RogueEventInstance eventInstance)
        {
            RunningEvent.Remove(eventInstance);
        }

        public void FinishEvent(RogueEventInstance eventInstance)
        {
            eventInstance.Finish();
        }
        
        public void NpcDisappear(RogueEventInstance eventInstance)
        {
            RunningEvent.Remove(eventInstance);
            Player.SceneInstance!.RemoveEntity(eventInstance.EventEntity);
        }

        public RogueEventInstance? FindEvent(int optionId)
        {
            foreach (var eventInstance in RunningEvent)
            {
                if (eventInstance.Options.Exists(x => x.OptionId == optionId))
                {
                    return eventInstance;
                }
            }
            return null;
        }

        public void TriggerEvent(RogueEventInstance? eventInstance, int eventId)
        {
            GameData.DialogueEventData.TryGetValue(eventId, out var dialogueEvent);
            if (dialogueEvent == null) return;

            List<int> Param = dialogueEvent.RogueEffectParamList;

            // Handle option
            if (EffectHandler.TryGetValue(dialogueEvent.RogueEffectType, out var effectHandler))
            {
                effectHandler.Handle(Rogue, eventInstance, Param);
            }

            // Handle cost
            if (CostHandler.TryGetValue(dialogueEvent.CostType, out var costHandler))
            {
                costHandler.Handle(Rogue, eventInstance, dialogueEvent.CostParamList);
            }
        }

        public void SelectOption(RogueEventInstance eventInstance, int optionId)
        {
            eventInstance.SelectIds.Add(optionId);
            var option = eventInstance.Options.Find(x => x.OptionId == optionId);
            if (option == null)
            {
                Player.SendPacket(new PacketSelectRogueDialogueEventScRsp());
                return;
            }
            GameData.DialogueEventData.TryGetValue(option.OptionId, out var dialogueEvent);
            if (dialogueEvent == null)
            {
                Player.SendPacket(new PacketSelectRogueDialogueEventScRsp());
                return;
            }

            List<int> Param = dialogueEvent.RogueEffectParamList;
            if (option.ArgId > 0)
            {
                GameData.DialogueDynamicContentData.TryGetValue(dialogueEvent.DynamicContentID, out var dynamicContent);
                if (dynamicContent != null)
                {
                    dynamicContent.TryGetValue(option.ArgId, out var content);
                    if (content != null)
                    {
                        if (content.DynamicParamType == DialogueDynamicParamTypeEnum.ReplaceAll)
                        {
                            Param = content.DynamicParamList;
                        } else
                        {
                            Param[content.DynamicParamList[0] - 1] = content.DynamicParamList[1];
                        }
                    }
                }
            }

            // Handle option
            if (EffectHandler.TryGetValue(dialogueEvent.RogueEffectType, out var effectHandler))
            {
                effectHandler.Handle(Rogue, eventInstance, Param);
            }

            // Handle cost
            if (CostHandler.TryGetValue(dialogueEvent.CostType, out var costHandler))
            {
                costHandler.Handle(Rogue, eventInstance, dialogueEvent.CostParamList);
            }

            // send rsp
            Player.SendPacket(new PacketSelectRogueDialogueEventScRsp(eventInstance));
        }
    }
}
