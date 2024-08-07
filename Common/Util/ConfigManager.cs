using EggLink.DanhengServer.Configuration;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Util;

public static class ConfigManager
{
    public static Logger logger = new("ConfigManager");
    public static ConfigContainer Config { get; private set; } = new();

    public static void LoadConfig()
    {
        var file = new FileInfo("config.json");
        if (!file.Exists)
        {
            logger.Warn("Config file not found, creating a new one");
            Config = new ConfigContainer();
            Config.MuipServer.AdminKey = Guid.NewGuid().ToString();
            logger.Info("Muipserver Admin key: " + Config.MuipServer.AdminKey);
            SaveConfig();
        }

        using var reader = new StreamReader(file.OpenRead());
        var json = reader.ReadToEnd();
        Config = JsonConvert.DeserializeObject<ConfigContainer>(json)!;
    }

    public static void SaveConfig()
    {
        var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
        File.WriteAllText("config.json", json);
    }
}