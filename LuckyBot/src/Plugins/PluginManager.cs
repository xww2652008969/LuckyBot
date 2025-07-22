using System.Reflection;
using LuckyBot.Models;

namespace LuckyBot.Plugins;

public class PluginManager
{
    private readonly Dictionary<string, Plugin> Plugins = [];

    public PluginManager(BotConfig config) => LoadAllPlugins(config);

    private void LoadAllPlugins(BotConfig config)
    {
        var types = Assembly.GetEntryAssembly()!.GetTypes();
        foreach (var type in types)
        {
            if (typeof(Plugin).IsAssignableFrom(type) && !type.IsAbstract)
            {
                if (Activator.CreateInstance(type) is Plugin plugin)
                {
                    plugin.PluginManager = this;
                    plugin.Config = config;
                    var pluginName = plugin.GetPluginName();
                    if (!string.IsNullOrEmpty(pluginName))
                    {
                        Plugins.TryAdd(pluginName, plugin);
                        plugin.Init();
                    }
                }
            }
        }
    }
}
