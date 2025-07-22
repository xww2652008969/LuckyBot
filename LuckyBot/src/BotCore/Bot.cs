using LuckyBot.Event;
using LuckyBot.Models;
using LuckyBot.Plugins;
using LuckyBot.WebSocket;

namespace LuckyBot.BotCore;

public class Bot
{
    private readonly WebSocketWClient WsClient;
    private readonly Queue<Message> MessageQueue = [];
    private readonly PluginManager PluginManager;

    public Bot(BotConfig config)
    {
        WsClient = new WebSocketWClient(config, MessageQueue);
        PluginManager = new PluginManager(config);
    }

    public void Start()
    {
        WsClient.Connect();
        ReadMessage();
    }

    private void ReadMessage()
    {
        while (true)
        {
            if (MessageQueue.Count > 0)
            {
                var message = MessageQueue.Dequeue();
                GroupEvent.SendMessage(message);
                PrivateEvent.SendMessage(message);
                MessageSend.SendMessage(message);
                NoticeMessage.SendMessage(message);
            }

            Thread.Sleep(100);
        }
    }
}
