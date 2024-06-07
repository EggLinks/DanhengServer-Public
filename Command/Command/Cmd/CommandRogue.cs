using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Data.Excel;
using EggLink.DanhengServer.Game.Rogue.Scene.Entity;
using EggLink.DanhengServer.Internationalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("rogue", "Game.Command.Rogue.Desc", "Game.Command.Rogue.Usage")]
    public class CommandRogue : ICommand
    {
        [CommandMethod("0 money")]
        public void GetMoney(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var count = arg.GetInt(0);
            arg.Target.Player!.RogueManager!.GetRogueInstance()?.GainMoney(count);
            arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerGainedMoney", count.ToString()));
        }

        [CommandMethod("0 buff")]
        public void GetBuff(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var id = arg.GetInt(0);

            if (id == -1)
            {
                var buffList = new List<RogueBuffExcel>();
                foreach (var buff in GameData.RogueBuffData.Values)
                {
                    if (buff.IsAeonBuff || buff.MazeBuffLevel == 2) continue;
                    buffList.Add(buff);
                }
                arg.Target.Player!.RogueManager!.GetRogueInstance()?.AddBuffList(buffList);

                arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerGainedAllItems", I18nManager.Translate("Word.Buff")));
            }
            else
            {
                GameData.RogueMazeBuffData.TryGetValue(id, out var buff);
                if (buff == null)
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.ItemNotFound", I18nManager.Translate("Word.Buff")));
                    return;
                }
                arg.Target.Player!.RogueManager!.GetRogueInstance()?.AddBuff(buff.ID, buff.Lv);

                arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerGainedItem", I18nManager.Translate("Word.Buff"), buff.Name ?? id.ToString()));
            }
        }

        [CommandMethod("0 miracle")]
        public void GetMiracle(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var id = arg.GetInt(0);

            GameData.RogueMiracleData.TryGetValue(id, out var miracle);
            if (miracle == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.ItemNotFound", I18nManager.Translate("Word.Miracle")));
                return;
            }
            arg.Target.Player!.RogueManager!.GetRogueInstance()?.AddMiracle(id);
            arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerGainedItem", I18nManager.Translate("Word.Miracle"), miracle.Name ?? id.ToString()));
        }

        [CommandMethod("0 enhance")]
        public void GetEnhance(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }
            var id = arg.GetInt(0);
            if (id == -1)
            {
                foreach (var enhance in GameData.RogueBuffData.Values)
                {
                    arg.Target.Player!.RogueManager!.GetRogueInstance()?.EnhanceBuff(enhance.MazeBuffID);
                }
                arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerEnhancedAllBuffs"));
            }
            else
            {
                GameData.RogueMazeBuffData.TryGetValue(id, out var buff);
                if (buff == null)
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.ItemNotFound", I18nManager.Translate("Word.Buff")));
                    return;
                }
                arg.Target.Player!.RogueManager!.GetRogueInstance()?.EnhanceBuff(buff.ID);
                arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerEnhancedBuff", buff.Name ?? id.ToString()));
            }
        }

        [CommandMethod("0 unstuck")]
        public void Unstuck(CommandArg arg)
        {
            if (arg.Target == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            var player = arg.Target.Player!;
            foreach (var npc in player.SceneInstance!.Entities.Values)
            {
                if (npc is RogueNpc rNpc)
                {
                    if (rNpc.RogueNpcId > 0)
                    {
                        player.SceneInstance!.RemoveEntity(rNpc);
                    }
                }
            }

            arg.SendMsg(I18nManager.Translate("Game.Command.Rogue.PlayerUnstuck"));
        }
    }
}
