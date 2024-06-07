using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Database.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Util
{
    public static class GameTools
    {
        public static int GetRandomRelicMainAffix(int GroupId)
        {
            GameData.RelicMainAffixData.TryGetValue(GroupId, out var affixes);
            if (affixes == null)
            {
                return 0;
            }
            List<int> affixList = [];
            foreach (var affix in affixes.Values)
            {
                if (affix.IsAvailable)
                {
                    affixList.Add(affix.AffixID);
                }
            }
            if (affixList.Count == 0)
            {
                return 0;
            }
            return affixList.RandomElement();
        }
    }
}
