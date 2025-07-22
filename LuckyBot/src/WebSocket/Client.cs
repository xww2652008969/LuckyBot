using LuckyBot.Models;

namespace LuckyBot.WebSocket;

public class WebSocketWClient
{
    private WebSocketSharp.WebSocket Client { get; set; }

    private const int ReconnectDelayMs = 30_000;
    private readonly Queue<Message> MessageQueue;
    private readonly BotConfig BotConfig;
    private bool IsConnect;

    public WebSocketWClient(BotConfig botConfig, Queue<Message> messageQueue)
    {
        BotConfig = botConfig;
        MessageQueue = messageQueue;
        Client = new WebSocketSharp.WebSocket($"{BotConfig.WsUrl}");
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        Client.OnMessage += (sender, e) =>
        {
            var jsonStr = e.IsBinary ? System.Text.Encoding.UTF8.GetString(e.RawData) : e.Data;
            if (string.IsNullOrEmpty(jsonStr)) return;
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(jsonStr);
            if (message != null)
            {
                message.SetBotConfig(BotConfig);
                MessageQueue.Enqueue(message);
            }
        };
        Client.OnClose += (sender, e) =>
        {
            Console.WriteLine("WebSocket closed, try reconnecting...");
            TryReconnect().Wait();
        };
    }

    public void Connect() => Client.Connect();

    private async Task TryReconnect()
    {
        if (IsConnect) return;
        while (true)
        {
            IsConnect = true;
            try
            {
                Connect();
                if (Client.IsAlive)
                {
                    IsConnect = false;
                    Console.WriteLine("WebSocket reconnected successfully.");
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reconnect failed: {ex.Message}");
            }

            await Task.Delay(ReconnectDelayMs);
        }
    }
}
