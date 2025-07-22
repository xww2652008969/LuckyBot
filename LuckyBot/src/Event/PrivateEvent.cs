using LuckyBot.Models;

namespace LuckyBot.Event;

public static class PrivateEvent
{
    public delegate void PrivateEventHandler(Message message);

    public static event PrivateEventHandler? OnPrivateMessage;

    public static void SendMessage(Message message)
    {
        if (message is { PostType: "message", MessageType: "private" })
        {
            foreach (var handler in OnPrivateMessage?.GetInvocationList() ?? [])
            {
                var h = (PrivateEventHandler)handler;
                try
                {
                    h(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"事件处理异常: {ex.Message}");
                }
            }
        }
    }
}
