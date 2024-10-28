using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.GameServer.Game.Scene.Entity;
using EggLink.DanhengServer.Internationalization;

namespace EggLink.DanhengServer.Command.Command.Cmd;

[CommandInfo("scene", "Game.Command.Scene.Desc", "Game.Command.Scene.Usage")]
public class CommandScene : ICommand
{
    [CommandMethod("0 group")]
    public async ValueTask GetLoadedGroup(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var scene = arg.Target!.Player!.SceneInstance!;
        var loadedGroup = new List<int>();
        foreach (var group in scene.Entities)
            if (!loadedGroup.Contains(group.Value.GroupID))
                loadedGroup.Add(group.Value.GroupID);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.LoadedGroups", string.Join(", ", loadedGroup)));
    }

    [CommandMethod("0 prop")]
    public async ValueTask GetProp(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var scene = arg.Target!.Player!.SceneInstance!;
        EntityProp? prop = null;
        foreach (var entity in scene.GetEntitiesInGroup<EntityProp>(arg.GetInt(0)))
            if (entity.PropInfo.ID == arg.GetInt(1))
            {
                prop = entity;
                break;
            }

        if (prop == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.PropNotFound"));
            return;
        }

        await prop.SetState((PropStateEnum)arg.GetInt(2));
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.PropStateChanged", prop.PropInfo.ID.ToString(),
            prop.State.ToString()));
    }

    [CommandMethod("0 remove")]
    public async ValueTask RemoveEntity(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var scene = arg.Target!.Player!.SceneInstance!;
        scene.Entities.TryGetValue(arg.GetInt(0), out var entity);
        if (entity == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.EntityNotFound"));
            return;
        }

        await scene.RemoveEntity(entity);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.EntityRemoved", entity.EntityID.ToString()));
    }

    [CommandMethod("0 unlockall")]
    public async ValueTask UnlockAll(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var scene = arg.Target!.Player!.SceneInstance!;
        foreach (var entity in scene.Entities.Values)
            if (entity is EntityProp prop)
                if (prop.Excel.PropStateList.Contains(PropStateEnum.Open))
                    await prop.SetState(PropStateEnum.Open);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.AllPropsUnlocked"));
    }

    [CommandMethod("0 change")]
    public async ValueTask ChangeScene(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        if (arg.GetInt(0) == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var player = arg.Target!.Player!;
        await player.EnterScene(arg.GetInt(0), 0, true);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.SceneChanged", arg.GetInt(0).ToString()));
    }

    [CommandMethod("0 reload")]
    public async ValueTask ReloadScene(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target!.Player!;
        await player.EnterScene(player.Data.EntryId, 0, true);
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.SceneReloaded"));
    }

    [CommandMethod("0 reset")]
    public async ValueTask ResetFloor(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var floorId = arg.GetInt(0);
        if (floorId == 0)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.InvalidArguments"));
            return;
        }

        var player = arg.Target!.Player!;
        if (player.SceneData?.ScenePropData.TryGetValue(floorId, out _) == true)
            player.SceneData.ScenePropData[floorId] = [];

        if (player.SceneData?.FloorSavedData.TryGetValue(floorId, out _) == true)
            player.SceneData.FloorSavedData[floorId] = [];

        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.SceneReset", floorId.ToString()));
    }

    [CommandMethod("0 cur")]
    public async ValueTask GetCurrentScene(CommandArg arg)
    {
        if (arg.Target == null)
        {
            await arg.SendMsg(I18NManager.Translate("Game.Command.Notice.PlayerNotFound"));
            return;
        }

        var player = arg.Target!.Player!;
        await arg.SendMsg(I18NManager.Translate("Game.Command.Scene.CurrentScene", player.Data.EntryId.ToString(),
            player.Data.PlaneId.ToString(), player.Data.FloorId.ToString()));
    }
}