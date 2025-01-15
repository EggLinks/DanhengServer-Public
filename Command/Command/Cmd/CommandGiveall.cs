using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Enums.Avatar;
using EggLink.DanhengServer.Enums.Item;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.PlayerSync;
using EggLink.DanhengServer.GameServer.Server.Packet.Send.Scene;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("giveall", "Game.Command.GiveAll.Desc", "Game.Command.GiveAll.Usage", ["ga"])]
public class CommandGiveall : ICommand
{
    [CommandMethod("0 avatar")]
    public async ValueTask GiveAllAvatar(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        arg.CharacterArgs.TryGetValue("r", out var rankStr);
        arg.CharacterArgs.TryGetValue("l", out var levelStr);
        rankStr ??= "1";
        levelStr ??= "1";
        if (!int.TryParse(rankStr, out var rank) || !int.TryParse(levelStr, out var level))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var avatarList = GameData.AvatarConfigData.Values;
        foreach (var avatar in avatarList)
        {
            if (avatar.AvatarID > 2000 && avatar.AvatarID != 8001)
                continue; // Hacky way to prevent giving random avatars
            if (player.AvatarManager!.GetAvatar(avatar.AvatarID) == null)
            {
                GameData.MultiplePathAvatarConfigData.TryGetValue(avatar.AvatarID, out var multiPathAvatar);
                if (multiPathAvatar != null && avatar.AvatarID != multiPathAvatar.BaseAvatarID) continue;
                // Normal avatar
                await player.InventoryManager!.AddItem(avatar.AvatarID, 1, false, sync: false);
                player.AvatarManager!.GetAvatar(avatar.AvatarID)!.Level = Math.Max(Math.Min(level, 80), 0);
                player.AvatarManager!.GetAvatar(avatar.AvatarID)!.Promotion =
                    GameData.GetMinPromotionForLevel(Math.Max(Math.Min(level, 80), 0));
                player.AvatarManager!.GetAvatar(avatar.AvatarID)!.GetCurPathInfo().Rank =
                    Math.Max(Math.Min(rank, 6), 0);
            }
            else
            {
                player.AvatarManager!.GetAvatar(avatar.AvatarID)!.Level = Math.Max(Math.Min(level, 80), 0);
                player.AvatarManager!.GetAvatar(avatar.AvatarID)!.Promotion =
                    GameData.GetMinPromotionForLevel(Math.Max(Math.Min(level, 80), 0));
                player.AvatarManager!.GetAvatar(avatar.AvatarID)!.GetCurPathInfo().Rank =
                    Math.Max(Math.Min(rank, 6), 0);
            }
        }

        await player.SendPacket(new PacketPlayerSyncScNotify(player.AvatarManager!.AvatarData.Avatars));

        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Avatar"), "1"));
    }

    [CommandMethod("0 equipment")]
    public async ValueTask GiveAllLightcone(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        arg.CharacterArgs.TryGetValue("r", out var rankStr);
        arg.CharacterArgs.TryGetValue("l", out var levelStr);
        arg.CharacterArgs.TryGetValue("x", out var amountStr);
        rankStr ??= "1";
        levelStr ??= "1";
        amountStr ??= "1";
        if (!int.TryParse(rankStr, out var rank) || !int.TryParse(levelStr, out var level) ||
            !int.TryParse(amountStr, out var amount))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var lightconeList = GameData.EquipmentConfigData.Values;
        var items = new List<ItemData>();

        for (var i = 0; i < amount; i++)
            foreach (var lightcone in lightconeList)
            {
                var item = await player.InventoryManager!.AddItem(lightcone.EquipmentID, 1, false,
                    Math.Max(Math.Min(rank, 5), 0), Math.Max(Math.Min(level, 80), 0), false);

                if (item != null)
                    items.Add(item);
            }

        await player.SendPacket(new PacketPlayerSyncScNotify(items));

        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Equipment"), amount.ToString()));
    }

    [CommandMethod("0 material")]
    public async ValueTask GiveAllMaterial(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        arg.CharacterArgs.TryGetValue("x", out var amountStr);
        amountStr ??= "1";
        if (!int.TryParse(amountStr, out var amount))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var materialList = GameData.ItemConfigData.Values;
        var items = new List<ItemData>();
        foreach (var material in materialList)
            if (material.ItemMainType == ItemMainTypeEnum.Material || material.ItemSubType == ItemSubTypeEnum.Food)
                items.Add(new ItemData
                {
                    ItemId = material.ID,
                    Count = amount
                });

        await player.InventoryManager!.AddItems(items, false);
        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Material"), amount.ToString()));
    }

    [CommandMethod("0 pet")]
    public async ValueTask GiveAllPet(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        arg.CharacterArgs.TryGetValue("x", out var amountStr);
        amountStr ??= "1";
        if (!int.TryParse(amountStr, out var amount))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var petList = GameData.ItemConfigData.Values;
        var items = new List<ItemData>();
        foreach (var pet in petList)
            if (pet.ItemMainType == ItemMainTypeEnum.Pet)
                items.Add(new ItemData
                {
                    ItemId = pet.ID,
                    Count = amount
                });
        await player.InventoryManager!.AddItems(items);
        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Pet"), "1"));
    }

    [CommandMethod("0 relic")]
    public async ValueTask GiveAllRelic(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        arg.CharacterArgs.TryGetValue("l", out var levelStr);
        levelStr ??= "1";
        if (!int.TryParse(levelStr, out var level))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        arg.CharacterArgs.TryGetValue("x", out var amountStr);
        amountStr ??= "1";
        if (!int.TryParse(amountStr, out var amount))
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var relicList = GameData.RelicConfigData.Values;
        var items = new List<ItemData>();

        for (var i = 0; i < amount; i++)
            foreach (var relic in relicList)
            {
                var item = await player.InventoryManager!.AddItem(relic.ID, amount, false, 1,
                    Math.Max(Math.Min(level, relic.MaxLevel), 1), false);

                if (item != null)
                    items.Add(item);
            }

        await player.SendPacket(new PacketPlayerSyncScNotify(items));

        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Relic"), amount.ToString()));
    }

    [CommandMethod("0 unlock")]
    public async ValueTask GiveAllUnlock(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var materialList = GameData.ItemConfigData.Values;
        foreach (var material in materialList)
            if (material.ItemMainType == ItemMainTypeEnum.Usable)
                if (material.ItemSubType == ItemSubTypeEnum.HeadIcon ||
                    material.ItemSubType == ItemSubTypeEnum.PhoneTheme ||
                    material.ItemSubType == ItemSubTypeEnum.ChatBubble)
                    await player.InventoryManager!.AddItem(material.ID, 1, false);

        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Unlock"), "1"));
    }

    [CommandMethod("0 train")]
    public async ValueTask GiveAllTrainItem(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        foreach (var grid in GameData.TrainPartyGridConfigData.Keys) await player.TrainPartyManager!.AddGrid(grid);

        foreach (var card in GameData.TrainPartyCardConfigData.Keys) await player.TrainPartyManager!.AddCard(card);

        foreach (var area in player.TrainPartyManager!.TrainPartyData.Areas)
        foreach (var step in GameData.TrainPartyStepConfigData.Values.Where(stepExcel => GameData
                     .TrainPartyAreaGoalConfigData.First(x => x.Value.AreaID == area.Value.AreaId).Value
                     .StepGroupList.Contains(stepExcel.GroupID)))
            area.Value.StepList.Add(step.ID);

        Dictionary<string, int> update = [];
        player.SceneData!.FloorSavedData[player.SceneInstance!.FloorId] = [];
        foreach (var savedValue in player.SceneInstance!.FloorInfo!.FloorSavedValue)
            if (savedValue.Name.StartsWith("Build_") || savedValue.Name == "Onboarded")
            {
                player.SceneData!.FloorSavedData[player.SceneInstance!.FloorId][savedValue.Name] = 1;
                update.TryAdd(savedValue.Name, 1);
            }
            else if (savedValue.Name.StartsWith("Progress_"))
            {
                player.SceneData!.FloorSavedData[player.SceneInstance!.FloorId][savedValue.Name] = 100;
                update.TryAdd(savedValue.Name, 100);
            }

        await player.SendPacket(new PacketUpdateFloorSavedValueNotify(update, player));

        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.TrainItem"), "1"));
    }

    [CommandMethod("0 path")]
    public async ValueTask GiveAllPath(CommandArg arg)
    {
        var player = arg.Target?.Player;
        if (player == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        foreach (var multiPathAvatar in GameData.MultiplePathAvatarConfigData.Values)
        {
            if (player.AvatarManager!.GetAvatar(multiPathAvatar.BaseAvatarID) == null)
            {
                await player.InventoryManager!.AddItem(multiPathAvatar.BaseAvatarID, 1, false, sync: false);
                player.AvatarManager!.GetAvatar(multiPathAvatar.BaseAvatarID)!.Level = Math.Max(Math.Min(1, 80), 0);
                player.AvatarManager!.GetAvatar(multiPathAvatar.BaseAvatarID)!.Promotion =
                    GameData.GetMinPromotionForLevel(Math.Max(Math.Min(1, 80), 0));
                player.AvatarManager!.GetAvatar(multiPathAvatar.BaseAvatarID)!.GetCurPathInfo().Rank =
                    Math.Max(Math.Min(0, 6), 0);
            }

            var avatarData = player.AvatarManager!.GetAvatar(multiPathAvatar.BaseAvatarID)!;
            if (avatarData.PathInfoes.ContainsKey(multiPathAvatar.AvatarID)) continue;
            if (multiPathAvatar.BaseAvatarID > 8000 && multiPathAvatar.AvatarID % 2 != 1) continue;
            await player.ChangeAvatarPathType(multiPathAvatar.BaseAvatarID,
                (MultiPathAvatarTypeEnum)multiPathAvatar.AvatarID);
        }

        await player.SendPacket(new PacketPlayerSyncScNotify(player.AvatarManager!.AvatarData.Avatars));

        await arg.SendMsg(I18NManager.Translate("Game.Command.GiveAll.GiveAllItems",
            I18NManager.Translate("Word.Avatar"),
            "1"));
    }
}