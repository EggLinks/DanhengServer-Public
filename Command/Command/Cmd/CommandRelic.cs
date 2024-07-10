using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Inventory;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Command.Cmd
{
    [CommandInfo("relic", "Game.Command.Relic.Desc", "Game.Command.Relic.Usage")]
    public class CommandRelic : ICommand
    {
        [CommandDefault]
        public void GiveRelic(CommandArg arg)
        {
            var player = arg.Target?.Player;
            if (player == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.PlayerNotFound"));
                return;
            }

            if (arg.BasicArgs.Count < 3)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            arg.CharacterArgs.TryGetValue("x", out var str);
            arg.CharacterArgs.TryGetValue("l", out var levelStr);
            str ??= "1";
            levelStr ??= "1";
            if (!int.TryParse(str, out var amount) || !int.TryParse(levelStr, out var level))
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                return;
            }

            GameData.RelicConfigData.TryGetValue(int.Parse(arg.BasicArgs[0]), out var itemConfig);
            GameData.ItemConfigData.TryGetValue(int.Parse(arg.BasicArgs[0]), out var itemConfigExcel);
            if (itemConfig == null || itemConfigExcel == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Relic.RelicNotFound"));
                return;
            }

            GameData.RelicSubAffixData.TryGetValue(itemConfig.SubAffixGroup, out var subAffixConfig);
            GameData.RelicMainAffixData.TryGetValue(itemConfig.MainAffixGroup, out var mainAffixConfig);
            if (subAffixConfig == null || mainAffixConfig == null)
            {
                arg.SendMsg(I18nManager.Translate("Game.Command.Relic.RelicNotFound"));
                return;
            }

            int startIndex = 1;
            int mainAffixId;
            if (arg.BasicArgs[1].Contains(':'))
            {
                // 随机主词条
                mainAffixId = mainAffixConfig.Keys.ToList().RandomElement();
            }
            else
            {
                mainAffixId = int.Parse(arg.BasicArgs[1]);
                if (!mainAffixConfig.ContainsKey(mainAffixId))
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Relic.InvalidMainAffixId"));
                    return;
                }
                startIndex++;
            }

            var remainLevel = 5;
            var subAffixes = new List<(int, int)>();
            for (var i = startIndex; i < arg.BasicArgs.Count; i++)
            {
                var subAffix = arg.BasicArgs[i].Split(':');
                if (subAffix.Length != 2 || !int.TryParse(subAffix[0], out var subId) || !int.TryParse(subAffix[1], out var subLevel))
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Notice.InvalidArguments"));
                    return;
                }
                if (!subAffixConfig.ContainsKey(subId))
                {
                    arg.SendMsg(I18nManager.Translate("Game.Command.Relic.InvalidSubAffixId"));
                    return;
                }
                subAffixes.Add((subId, subLevel));
                remainLevel -= subLevel - 1;
            }
            if (subAffixes.Count < 4)
            {
                // 随机副词条
                var subAffixGroup = itemConfig.SubAffixGroup;
                var subAffixGroupConfig = GameData.RelicSubAffixData[subAffixGroup];
                var subAffixGroupKeys = subAffixGroupConfig.Keys.ToList();
                while (subAffixes.Count < 4)
                {
                    var subId = subAffixGroupKeys.RandomElement();
                    if (subAffixes.Any(x => x.Item1 == subId))
                    {
                        continue;
                    }
                    if (remainLevel <= 0)
                    {
                        subAffixes.Add((subId, 1));
                    }
                    else
                    {
                        var subLevel = Random.Shared.Next(1, Math.Min(remainLevel + 1, 5)) + 1;
                        subAffixes.Add((subId, subLevel));
                        remainLevel -= subLevel - 1;
                    }
                }
            }

            var itemData = new ItemData()
            {
                ItemId = int.Parse(arg.BasicArgs[0]),
                Level = Math.Max(Math.Min(level, 15), 1),
                UniqueId = ++player.InventoryManager!.Data.NextUniqueId,
                MainAffix = mainAffixId,
                Count = 1,
            };

            foreach (var (subId, subLevel) in subAffixes)
            {
                subAffixConfig.TryGetValue(subId, out var subAffix);
                var aff = new ItemSubAffix(subAffix!, 1);
                for (var i = 1; i < subLevel; i++)
                {
                    aff.IncreaseStep(subAffix!.StepNum);
                }
                itemData.SubAffixes.Add(aff);
            }

            for (var i = 0; i < amount; i++)
            {
                player.InventoryManager!.AddItem(itemData, notify: false);
            }

            arg.SendMsg(I18nManager.Translate("Game.Command.Relic.RelicGiven", player.Uid.ToString(), amount.ToString(), itemConfigExcel.Name ?? itemData.ItemId.ToString(), itemData.MainAffix.ToString()));
        }
    }
}
