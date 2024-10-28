using System.Reflection;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Enums.Rogue;
using EggLink.DanhengServer.GameServer.Game.Player;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.RogueCommon;

namespace EggLink.DanhengServer.GameServer.Game.Rogue.Event;

public class RogueEventManager
{
    public Dictionary<DialogueEventCostTypeEnum, RogueEventCostHandler> CostHandler = [];
    public Dictionary<DialogueEventTypeEnum, RogueEventEffectHandler> EffectHandler = [];
    public PlayerInstance Player;
    public BaseRogueInstance Rogue;
    public List<RogueEventInstance> RunningEvent = [];

    public RogueEventManager(PlayerInstance player, BaseRogueInstance rogueInstance)
    {
        Player = player;
        Rogue = rogueInstance;

        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<RogueEventAttribute>();
            if (attr == null) continue;
            if (attr.EffectType != DialogueEventTypeEnum.None)
            {
                // Effect
                var effect = (RogueEventEffectHandler)Activator.CreateInstance(type, null)!;
                EffectHandler.Add(attr.EffectType, effect);
            }
            else
            {
                // Cost
                var cost = (RogueEventCostHandler)Activator.CreateInstance(type, null)!;
                CostHandler.Add(attr.CostType, cost);
            }
        }
    }

    public void OnNextRoom()
    {
        RunningEvent.Clear(); // Clear all running events
    }

    public async ValueTask AddEvent(RogueEventInstance eventInstance)
    {
        RunningEvent.Add(eventInstance);
        foreach (var option in eventInstance.Options)
        {
            GameData.DialogueEventData.TryGetValue(option.OptionId, out var dialogueEvent);
            if (dialogueEvent == null) continue;

            var param = dialogueEvent.RogueEffectParamList;

            // Init option
            if (EffectHandler.TryGetValue(dialogueEvent.RogueEffectType, out var effectHandler))
                effectHandler.Init(Rogue, eventInstance, param, option);
        }

        await Player.SendPacket(new PacketSyncRogueCommonDialogueDataScNotify(eventInstance));
    }

    public void RemoveEvent(RogueEventInstance eventInstance)
    {
        RunningEvent.Remove(eventInstance);
    }

    public async ValueTask FinishEvent(RogueEventInstance eventInstance)
    {
        await eventInstance.Finish();
    }

    public async ValueTask NpcDisappear(RogueEventInstance eventInstance)
    {
        RunningEvent.Remove(eventInstance);
        await Player.SceneInstance!.RemoveEntity(eventInstance.EventEntity);
    }

    public RogueEventInstance? FindEvent(int optionId)
    {
        return RunningEvent.FirstOrDefault(eventInstance => eventInstance.Options.Exists(x => x.OptionId == optionId));
    }

    public async ValueTask TriggerEvent(RogueEventInstance? eventInstance, int eventId)
    {
        GameData.DialogueEventData.TryGetValue(eventId, out var dialogueEvent);
        if (dialogueEvent == null) return;

        var param = dialogueEvent.RogueEffectParamList;

        // Handle cost
        if (CostHandler.TryGetValue(dialogueEvent.CostType, out var costHandler))
            await costHandler.Handle(Rogue, eventInstance, dialogueEvent.CostParamList);

        // Handle option
        if (EffectHandler.TryGetValue(dialogueEvent.RogueEffectType, out var effectHandler))
            await effectHandler.Handle(Rogue, eventInstance, param, null);
    }

    public async ValueTask SelectOption(RogueEventInstance eventInstance, int optionId)
    {
        eventInstance.SelectedOptionId = optionId;
        var option = eventInstance.Options.Find(x => x.OptionId == optionId);
        if (option == null)
        {
            await Player.SendPacket(new PacketSelectRogueCommonDialogueOptionScRsp());
            return;
        }

        GameData.DialogueEventData.TryGetValue(option.OptionId, out var dialogueEvent);
        if (dialogueEvent == null)
        {
            await Player.SendPacket(new PacketSelectRogueCommonDialogueOptionScRsp());
            return;
        }

        await Player.SendPacket(new PacketSyncRogueCommonDialogueDataScNotify(eventInstance));

        var param = dialogueEvent.RogueEffectParamList;
        if (option.ArgId > 0)
        {
            GameData.DialogueDynamicContentData.TryGetValue(dialogueEvent.DynamicContentID, out var dynamicContent);
            if (dynamicContent != null)
            {
                dynamicContent.TryGetValue(option.ArgId, out var content);
                if (content != null)
                {
                    if (content.DynamicParamType == DialogueDynamicParamTypeEnum.ReplaceAll)
                        param = content.DynamicParamList;
                    else
                        param[content.DynamicParamList[0] - 1] = content.DynamicParamList[1];
                }
            }
        }

        // Handle cost
        if (CostHandler.TryGetValue(dialogueEvent.CostType, out var costHandler))
            await costHandler.Handle(Rogue, eventInstance, dialogueEvent.CostParamList);

        // Handle option
        if (EffectHandler.TryGetValue(dialogueEvent.RogueEffectType, out var effectHandler))
            await effectHandler.Handle(Rogue, eventInstance, param, option);

        // send rsp
        await Player.SendPacket(new PacketSyncRogueCommonDialogueOptionFinishScNotify(eventInstance));
        option.IsSelected = true;
        await Player.SendPacket(new PacketSelectRogueCommonDialogueOptionScRsp(eventInstance));
    }
}