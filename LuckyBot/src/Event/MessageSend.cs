using LuckyBot.Models;

namespace LuckyBot.Event;

public static class MessageSend
{
    public delegate void MessageSendHandler(Message message);

    public static event MessageSendHandler? OnMessageSend;

    public static void SendMessage(Message message)
    {
        if (message.PostType == "message_sent")
        {
            foreach (var handler in OnMessageSend?.GetInvocationList() ?? [])
            {
                var h = (MessageSendHandler)handler;
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
