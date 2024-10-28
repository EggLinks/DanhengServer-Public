using System.Text;
using EggLink.DanhengServer.Data;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Program.Program;
using EggLink.DanhengServer.Util;
using Newtonsoft.Json;

namespace EggLink.DanhengServer.Program.Handbook;

public static class HandbookGenerator
{
    public static readonly string HandbookPath = "Config/Handbook.txt";

    public static void Generate()
    {
        var config = ConfigManager.Config;
        var textMapPath = config.Path.ResourcePath + "/TextMap/TextMap" + config.ServerOption.Language + ".json";
        if (!File.Exists(textMapPath))
        {
            Logger.GetByClassName().Error(I18NManager.Translate("Server.ServerInfo.FailedToReadItem", textMapPath,
                I18NManager.Translate("Word.NotFound")));
            return;
        }

        var textMap = JsonConvert.DeserializeObject<Dictionary<long, string>>(File.ReadAllText(textMapPath));

        if (textMap == null)
        {
            Logger.GetByClassName().Error(I18NManager.Translate("Server.ServerInfo.FailedToReadItem", textMapPath,
                I18NManager.Translate("Word.Error")));
            return;
        }

        var builder = new StringBuilder();
        builder.AppendLine("Handbook generated in " + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
        builder.AppendLine();
        builder.AppendLine("#Command");
        builder.AppendLine();
        GenerateCmd(builder);

        builder.AppendLine();
        builder.AppendLine("#Avatar");
        builder.AppendLine();
        GenerateAvatar(builder, textMap);

        builder.AppendLine();
        builder.AppendLine("#Item");
        builder.AppendLine();
        GenerateItem(builder, textMap);

        builder.AppendLine();
        builder.AppendLine("#MainMission");
        builder.AppendLine();
        GenerateMainMissionId(builder, textMap);

        builder.AppendLine();
        builder.AppendLine("#SubMission");
        builder.AppendLine();
        GenerateSubMissionId(builder, textMap);

        builder.AppendLine();
        builder.AppendLine("#RogueBuff");
        builder.AppendLine();
        GenerateRogueBuff(builder, textMap);

        builder.AppendLine();
        builder.AppendLine("#RogueMiracle");
        builder.AppendLine();
        GenerateRogueMiracleDisplay(builder, textMap);

#if DEBUG
        builder.AppendLine();
        builder.AppendLine("#RogueDiceSurface");
        builder.AppendLine();
        GenerateRogueDiceSurfaceDisplay(builder, textMap);
#endif

        builder.AppendLine();
        WriteToFile(builder.ToString());

        Logger.GetByClassName()
            .Info(I18NManager.Translate("Server.ServerInfo.GeneratedItem", I18NManager.Translate("Word.Handbook")));
    }

    public static void GenerateCmd(StringBuilder builder)
    {
        foreach (var cmd in EntryPoint.CommandManager.CommandInfo)
        {
            builder.Append("\t" + cmd.Key);
            var desc = I18NManager.Translate(cmd.Value.Description).Replace("\n", "\n\t\t");
            builder.AppendLine(": " + desc);
        }
    }

    public static void GenerateItem(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var item in GameData.ItemConfigData.Values)
        {
            var name = map.TryGetValue(item.ItemName.Hash, out var value) ? value : $"[{item.ItemName.Hash}]";
            builder.AppendLine(item.ID + ": " + name);

            if (name != $"[{item.ItemName.Hash}]") item.Name = name;
        }
    }

    public static void GenerateAvatar(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var avatar in GameData.AvatarConfigData.Values)
        {
            var name = map.TryGetValue(avatar.AvatarName.Hash, out var value) ? value : $"[{avatar.AvatarName.Hash}]";
            builder.AppendLine(avatar.AvatarID + ": " + name);

            if (name != $"[{avatar.AvatarName.Hash}]") avatar.Name = name;
        }
    }

    public static void GenerateMainMissionId(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var mission in GameData.MainMissionData.Values)
        {
            var name = map.TryGetValue(mission.Name.Hash, out var value) ? value : $"[{mission.Name.Hash}]";
            builder.AppendLine(mission.MainMissionID + ": " + name);
        }
    }

    public static void GenerateSubMissionId(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var mission in GameData.SubMissionData.Values)
        {
            var name = map.TryGetValue(mission.TargetText.Hash, out var value) ? value : $"[{mission.TargetText.Hash}]";
            builder.AppendLine(mission.SubMissionID + ": " + name);
        }
    }

    public static void GenerateRogueBuff(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var buff in GameData.RogueMazeBuffData)
        {
            var name = map.TryGetValue(buff.Value.BuffName.Hash, out var value)
                ? value
                : $"[{buff.Value.BuffName.Hash}]";
            builder.AppendLine(buff.Key + ": " + name + " --- Level:" + buff.Value.Lv);

            if (name != $"[{buff.Value.BuffName.Hash}]") buff.Value.Name = name;
        }
    }

    public static void GenerateRogueMiracleDisplay(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var display in GameData.RogueMiracleData.Values)
        {
            var name = map.TryGetValue(display.MiracleName.Hash, out var value)
                ? value
                : $"[{display.MiracleName.Hash}]";
            builder.AppendLine(display.MiracleID + ": " + name);

            if (name != $"[{display.MiracleName.Hash}]") display.Name = name;
        }
    }

#if DEBUG
    public static void GenerateRogueDiceSurfaceDisplay(StringBuilder builder, Dictionary<long, string> map)
    {
        foreach (var display in GameData.RogueNousDiceSurfaceData.Values)
        {
            var name = map.TryGetValue(display.SurfaceName.Hash, out var value)
                ? value
                : $"[{display.SurfaceName.Hash}]";
            var desc = map.TryGetValue(display.SurfaceDesc.Hash, out var c) ? c : $"[{display.SurfaceDesc.Hash}]";
            builder.AppendLine(display.SurfaceID + ": " + name + "\n" + "Desc: " + desc);
        }
    }
#endif

    public static void WriteToFile(string content)
    {
        File.WriteAllText(HandbookPath, content);
    }
}