using EggLink.DanhengServer.Enums.Scene;
using EggLink.DanhengServer.Game.Scene.Entity;
using EggLink.DanhengServer.Internationalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("scene", "Game.Command.Scene.Desc", "Game.Command.Scene.Usage")]
    public class CommandScene : ICommand
    {
        [CommandMethod("0 group")]
        public void GetLoadedGroup(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var scene = arg.Target!.Player!.SceneInstance!;
            var loadedGroup = new List<int>();
            foreach (var group in scene.Entities)
            {
                if (!loadedGroup.Contains(group.Value.GroupID))
                {
                    loadedGroup.Add(group.Value.GroupID);
                }
            }
            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.LoadedGroups", string.Join(", ", loadedGroup)));
        }

        [CommandMethod("0 prop")]
        public void GetProp(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var scene = arg.Target!.Player!.SceneInstance!;
            EntityProp? prop = null;
            foreach (var entity in scene.GetEntitiesInGroup<EntityProp>(arg.GetInt(0)))
            {
                if (entity.PropInfo.ID == arg.GetInt(1))
                {
                    prop = entity;
                    break;
                }
            }
            if (prop == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Scene.PropNotFound"));
                return;
            }
            prop.SetState((PropStateEnum)arg.GetInt(2));
            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.PropStateChanged", prop.PropInfo.ID.ToString(), prop.State.ToString()));
        }

        [CommandMethod("0 remove")]
        public void RemoveEntity(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var scene = arg.Target!.Player!.SceneInstance!;
            scene.Entities.TryGetValue(arg.GetInt(0), out var entity);
            if (entity == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Scene.EntityNotFound"));
                return;
            }
            scene.RemoveEntity(entity);
            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.EntityRemoved", entity.EntityID.ToString()));
        }

        [CommandMethod("0 unlockall")]
        public void UnlockAll(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var scene = arg.Target!.Player!.SceneInstance!;
            foreach (var entity in scene.Entities.Values)
            {
                if (entity is EntityProp prop)
                {
                    if (prop.Excel.PropStateList.Contains(PropStateEnum.Open))
                        prop.SetState(PropStateEnum.Open);
                }
            }
            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.AllPropsUnlocked"));
        }

        [CommandMethod("0 change")]
        public void ChangeScene(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            if (arg.GetInt(0) == 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            var player = arg.Target!.Player!;
            player.EnterScene(arg.GetInt(0), 0, true);
            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.SceneChanged", arg.GetInt(0).ToString()));
        }

        [CommandMethod("0 reload")]
        public void ReloadScene(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            var player = arg.Target!.Player!;
            player.EnterScene(player.Data.EntryId, 0, true);
            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.SceneReloaded"));
        }

        [CommandMethod("0 reset")]
        public void ResetFloor(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            var floorId = arg.GetInt(0);
            if (floorId == 0)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            var player = arg.Target!.Player!;
            if (player.SceneData?.ScenePropData.TryGetValue(floorId, out var _) == true)
            {
                player.SceneData.ScenePropData[floorId] = [];
            }

            arg.SendMsg(I18nManager.Translate("Game.Command.Scene.SceneReset", floorId.ToString()));
        }
    }
}
