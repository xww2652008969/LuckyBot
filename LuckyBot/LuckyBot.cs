using LuckyBot.BotCore;
using LuckyBot.Models;
using LuckyBot.Plugins;
using LuckyBot.WebSocket;

namespace LuckyBot;

public static class LuckyBot
{
    public static void Mian()
    {
        var config = new BotConfig()
        {
            WsUrl = "ws://127.0.0.1:3001",
            HttpUrl = "http://127.0.1:3000",
        };
        var bot = new Bot(config);
        bot.Start();
    }
}
