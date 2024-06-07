using EggLink.DanhengServer.Plugin.Constructor;
using EggLink.DanhengServer.Util;
using McMaster.NETCore.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EggLink.DanhengServer.Plugin
{
    public class PluginManager
    {
        private readonly static Logger logger = new("PluginManager");
        public readonly static Dictionary<IPlugin, PluginInfo> Plugins = [];

        public readonly static Dictionary<IPlugin, List<Type>> PluginAssemblies = [];

        #region Plugin Manager

        public static void LoadPlugins()
        {
            // get all the plugins in the plugin directory
            if (!Directory.Exists(ConfigManager.Config.Path.PluginPath))
            {
                Directory.CreateDirectory(ConfigManager.Config.Path.PluginPath);
            }

            if (!Directory.Exists(ConfigManager.Config.Path.PluginConfigPath))
            {
                Directory.CreateDirectory(ConfigManager.Config.Path.PluginConfigPath);
            }

            var plugins = Directory.GetFiles(ConfigManager.Config.Path.PluginPath, "*.dll");
            var loaders = new List<PluginLoader>();
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var assemblyName = new AssemblyName(args.Name).Name + ".dll";
                var assemblyPath = Path.Combine(ConfigManager.Config.Path.PluginPath, assemblyName);

                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }

                return null;
            };
            foreach (var plugin in plugins)
            {
                var fileInfo = new FileInfo(plugin);
                LoadPlugin(fileInfo.FullName);
            }
        }

        public static void LoadPlugin(string plugin)
        {
            try
            {
                var config = new PluginConfig(plugin)
                {
                    PreferSharedTypes = true,
                    LoadInMemory = true,
                };

                var loader = new PluginLoader(config);

                var assembly = loader.LoadDefaultAssembly();
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (typeof(IPlugin).IsAssignableFrom(type))
                    {
                        if (Activator.CreateInstance(type) is IPlugin pluginInstance)
                        {
                            var pluginInfo = type.GetCustomAttribute<PluginInfo>();
                            if (pluginInfo != null)
                            {
                                logger.Info($"Loaded plugin {pluginInfo.Name} v{pluginInfo.Version}: {pluginInfo.Description}");
                            }
                            else
                            {
                                logger.Info($"Loaded plugin {plugin}: No plugin info");
                                continue;
                            }

                            if (Plugins.Values.Any(p => p.Name == pluginInfo.Name))
                            {
                                logger.Error($"Failed to load plugin {plugin}: Plugin already loaded");
                                continue;
                            }

                            Plugins.Add(pluginInstance, pluginInfo);

                            if (!PluginAssemblies.TryGetValue(pluginInstance, out var pluginTypes))
                            {
                                pluginTypes = [];
                                PluginAssemblies[pluginInstance] = pluginTypes;
                            }

                            pluginTypes.AddRange(types);

                            pluginInstance.OnLoad();
                        }
                        else
                        {
                            logger.Error($"Failed to load plugin {plugin}: Plugin instance is null");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to load plugin {plugin}: {ex.Message}");
            }
        }

        public static void UnloadPlugin(IPlugin plugin)
        {
            if (Plugins.TryGetValue(plugin, out PluginInfo? value))
            {
                plugin.OnUnload();
                Plugins.Remove(plugin);
                PluginAssemblies.Remove(plugin);
                logger.Info($"Unloaded plugin {value.Name}");
            }
        }


        public static void UnloadPlugins()
        {
            foreach (var plugin in Plugins.Keys)
            {
                UnloadPlugin(plugin);
            }

            logger.Info("Unloaded all plugins");
        }

        #endregion

        public static List<Type> GetPluginAssemblies()
        {
            var assemblies = new List<Type>();
            foreach (var plugin in Plugins.Keys)
            {
                if (PluginAssemblies.TryGetValue(plugin, out List<Type>? value))
                {
                    assemblies.AddRange(value);
                }
            }

            return assemblies;
        }
    }
}
