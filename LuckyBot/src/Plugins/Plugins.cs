using LuckyBot.Models;
using LuckyBot.PostApi;
using LuckyBot.WebSocket;

namespace LuckyBot.Plugins;

public abstract class Plugin
{
    public PluginManager? PluginManager;
    public BotConfig Config;

    protected HttpPostApi GetPostApi() => new(Config);

    public virtual string GetVersion() => "1.0.0";

    public virtual string GetAuthor() => "xww";

    public abstract string GetPluginName();

    public abstract void Init();

    public abstract void UnInit();
}
