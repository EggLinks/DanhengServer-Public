using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EggLink.DanhengServer.Data.Config.Rogue;

public class RogueDialogueBaseConfigInfo
{
    public string OptionPath { get; set; } = string.Empty;
    public string DialoguePath { get; set; } = string.Empty;

    public LevelGraphConfigInfo? DialogueInfo { get; set; }
    public RogueDialogueEventConfigInfo? OptionInfo { get; set; }

    public void Loaded()
    {
        var logger = Logger.GetByClassName();
        if (!string.IsNullOrEmpty(OptionPath))
        {
            var path = ConfigManager.Config.Path.ResourcePath + "/" + OptionPath;
            var file = new FileInfo(path);
            if (!file.Exists) return;
            try
            {
                using var reader = file.OpenRead();
                using StreamReader reader2 = new(reader);
                var text = reader2.ReadToEnd();
                var info = JsonConvert.DeserializeObject<RogueDialogueEventConfigInfo>(text);
                OptionInfo = info;
            }
            catch (Exception ex)
            {
                logger.Error(
                    I18NManager.Translate("Server.ServerInfo.FailedToReadItem", file.Name,
                        I18NManager.Translate("Word.Error")), ex);
            }
        }

        if (!string.IsNullOrEmpty(DialoguePath))
        {
            var path = ConfigManager.Config.Path.ResourcePath + "/" + DialoguePath;
            var file = new FileInfo(path);
            if (!file.Exists) return;
            try
            {
                using var reader = file.OpenRead();
                using StreamReader reader2 = new(reader);
                var text = reader2.ReadToEnd().Replace("$type", "Type");
                var obj = JObject.Parse(text);
                var info = LevelGraphConfigInfo.LoadFromJsonObject(obj);
                DialogueInfo = info;
            }
            catch (Exception ex)
            {
                logger.Error(
                    I18NManager.Translate("Server.ServerInfo.FailedToReadItem", file.Name,
                        I18NManager.Translate("Word.Error")), ex);
            }
        }
    }
}