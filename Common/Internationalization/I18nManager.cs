using System.Reflection;
using EggLink.DanhengServer.Enums.Language;
using EggLink.DanhengServer.Internationalization.Message;
using EggLink.DanhengServer.Util;

namespace EggLink.DanhengServer.Internationalization;

public static class I18NManager
{
    public static Logger Logger = new("I18nManager");

    public static object Language { get; set; } = new LanguageEN();
    public static Dictionary<string, Dictionary<ProgramLanguageTypeEnum, object>> PluginLanguages { get; } = [];

    public static void LoadLanguage()
    {
        var languageStr = "EggLink.DanhengServer.Internationalization.Message.Language" +
                          ConfigManager.Config.ServerOption.Language;
        var languageType = Type.GetType(languageStr);
        if (languageType == null)
        {
            Logger.Warn("Language not found, fallback to EN");
            // fallback to English
            languageType = Type.GetType("EggLink.DanhengServer.Internationalization.Message.LanguageEN")!;
        }

        var language = Activator.CreateInstance(languageType) ?? throw new Exception("Language not found");
        Language = language;

        Logger.Info(Translate("Server.ServerInfo.LoadedItem", Translate("Word.Language")));
    }

    public static void LoadPluginLanguage(Dictionary<string, List<Type>> pluginAssemblies)
    {
        foreach (var (pluginName, types) in pluginAssemblies)
        {
            var languageType = types.FindAll(x => x.GetCustomAttribute<PluginLanguageAttribute>() != null);
            if (languageType.Count == 0) // no language to use
                continue;

            PluginLanguages.Add(pluginName, []);
            foreach (var type in languageType)
            {
                var attr = type.GetCustomAttribute<PluginLanguageAttribute>();
                if (attr == null) continue;

                var language = Activator.CreateInstance(type);
                if (language == null) continue;
                PluginLanguages[pluginName].Add(attr.LanguageType, language);
            }
        }
    }

    public static string Translate(string key, params string[] args)
    {
        var pluginLangs = PluginLanguages.Values;
        var langs = (from pluginLang in pluginLangs
            from o in pluginLang
            where o.Key == Enum.Parse<ProgramLanguageTypeEnum>(ConfigManager.Config.ServerOption.Language)
            select o.Value).ToList(); // get all plugin languages
        langs.Add(Language); // add server language

        var result = langs.Select(lang => GetNestedPropertyValue(lang, key)).OfType<string>().FirstOrDefault() ?? key;

        var index = 0;

        return args.Aggregate(result, (current, arg) => current.Replace("{" + index++ + "}", arg));
    }

    public static string TranslateAsCertainLang(string langStr, string key, params string[] args)
    {
        var languageStr = "EggLink.DanhengServer.Internationalization.Message.Language" +
                          langStr;
        var languageType = Type.GetType(languageStr) ??
                           Type.GetType("EggLink.DanhengServer.Internationalization.Message.LanguageEN")!;
        var language = Activator.CreateInstance(languageType) ?? throw new Exception("Language not found");

        List<object> langs = [language];

        var result = langs.Select(lang => GetNestedPropertyValue(lang, key)).OfType<string>().FirstOrDefault() ?? key;

        var index = 0;

        return args.Aggregate(result, (current, arg) => current.Replace("{" + index++ + "}", arg));
    }


    public static string? GetNestedPropertyValue(object? obj, string propertyName)
    {
        foreach (var part in propertyName.Split('.'))
        {
            if (obj == null) return null;

            var type = obj.GetType();
            var property = type.GetProperty(part);
            if (property == null) return null;

            obj = property.GetValue(obj, null);
        }

        return (string?)obj;
    }
}