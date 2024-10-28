using System.Globalization;
using EggLink.DanhengServer.Data;

namespace EggLink.DanhengServer.Util;

public static class UtilTools
{
    public static int GetRandomRelicMainAffix(int groupId)
    {
        GameData.RelicMainAffixData.TryGetValue(groupId, out var affixes);
        if (affixes == null) return 0;
        List<int> affixList = [];
        affixList.AddRange(from affix in affixes.Values where affix.IsAvailable select affix.AffixID);
        return affixList.Count == 0 ? 0 : affixList.RandomElement();
    }

    public static string GetCurrentLanguage()
    {
        var uiCulture = CultureInfo.CurrentUICulture;
        return uiCulture.Name switch
        {
            "zh-CN" => "CHS",
            "zh-TW" => "CHT",
            "ja-JP" => "JP",
            _ => "EN"
        };
    }
}