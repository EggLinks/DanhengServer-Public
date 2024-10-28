using System.Reflection;
using EggLink.DanhengServer.GameServer.Plugin.Constructor;
using EggLink.DanhengServer.Internationalization;
using EggLink.DanhengServer.Util;
using McMaster.NETCore.Plugins;

namespace EggLink.DanhengServer.GameServer.Plugin;

public class PluginManager
{
    private static readonly Logger Logger = new("PluginManager");
    public static readonly Dictionary<IPlugin, PluginInfo> Plugins = [];

    public static readonly Dictionary<IPlugin, List<Type>> PluginAssemblies = [];

    public static List<Type> GetPluginAssemblies()
    {
        var assemblies = new List<Type>();
        foreach (var plugin in Plugins.Keys)
            if (PluginAssemblies.TryGetValue(plugin, out var value))
                assemblies.AddRange(value);

        return assemblies;
    }

    #region Plugin Manager

    public static void LoadPlugins()
    {
        // get all the plugins in the plugin directory
        if (!Directory.Exists(ConfigManager.Config.Path.PluginPath))
            Directory.CreateDirectory(ConfigManager.Config.Path.PluginPath);

        var plugins = Directory.GetFiles(ConfigManager.Config.Path.PluginPath, "*.dll");
        AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";
            var assemblyPath = Path.Combine(ConfigManager.Config.Path.PluginPath, assemblyName);

            return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
        };
        foreach (var plugin in plugins)
        {
            var fileInfo = new FileInfo(plugin);
            LoadPlugin(fileInfo.FullName);
        }

        //var dict = PluginAssemblies.ToDictionary(pluginAssembly => Plugins[pluginAssembly.Key].Name, pluginAssembly => pluginAssembly.Value);

        //I18NManager.LoadPluginLanguage(dict);
    }

    public static void LoadPlugin(string plugin)
    {
        try
        {
            var config = new PluginConfig(plugin)
            {
                PreferSharedTypes = true,
                LoadInMemory = true
            };

            var loader = new PluginLoader(config);

            var assembly = loader.LoadDefaultAssembly();
            var types = assembly.GetTypes();

            foreach (var type in types)
                if (typeof(IPlugin).IsAssignableFrom(type))
                {
                    if (Activator.CreateInstance(type) is IPlugin pluginInstance)
                    {
                        var pluginInfo = type.GetCustomAttribute<PluginInfo>();
                        if (pluginInfo != null)
                        {
                            Logger.Info(
                                $"Loaded plugin {pluginInfo.Name} v{pluginInfo.Version}: {pluginInfo.Description}");
                        }
                        else
                        {
                            Logger.Info($"Loaded plugin {plugin}: No plugin info");
                            continue;
                        }

                        if (Plugins.Values.Any(p => p.Name == pluginInfo.Name))
                        {
                            Logger.Error($"Failed to load plugin {plugin}: Plugin already loaded");
                            continue;
                        }

                        Plugins.Add(pluginInstance, pluginInfo);

                        if (!PluginAssemblies.TryGetValue(pluginInstance, out var pluginTypes))
                        {
                            pluginTypes = [];
                            PluginAssemblies[pluginInstance] = pluginTypes;
                        }

                        pluginTypes.AddRange(types);

                        try
                        {
                            var dict = new Dictionary<string, List<Type>> { { pluginInfo.Name, pluginTypes } };

                            I18NManager.LoadPluginLanguage(dict);
                            pluginInstance.OnLoad();
                        }
                        catch (Exception e)
                        {
                            Logger.Error($"Failed to load plugin {plugin}: {e.Message}");
                            // unload the plugin
                            UnloadPlugin(pluginInstance);
                        }
                    }
                    else
                    {
                        Logger.Error($"Failed to load plugin {plugin}: Plugin instance is null");
                    }
                }
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load plugin {plugin}: {ex.Message}");
        }
    }

    public static void UnloadPlugin(IPlugin plugin)
    {
        if (!Plugins.TryGetValue(plugin, out var value)) return;
        plugin.OnUnload();
        Plugins.Remove(plugin);
        PluginAssemblies.Remove(plugin);
        Logger.Info($"Unloaded plugin {value.Name}");
    }


    public static void UnloadPlugins()
    {
        foreach (var plugin in Plugins.Keys) UnloadPlugin(plugin);

        Logger.Info(I18NManager.Translate("Server.ServerInfo.UnloadedItems", I18NManager.Translate("Word.Plugin")));
    }

    #endregion
}