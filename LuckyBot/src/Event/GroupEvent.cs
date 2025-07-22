using LuckyBot.Models;

namespace LuckyBot.Event;

public static class GroupEvent
{
    public delegate void GroupEventHandler(Message message);

    public static event GroupEventHandler? OnGroupMessage;

    public static void SendMessage(Message message)
    {
        if (message is { PostType: "message", MessageType: "group" })
        {
            foreach (var handler in OnGroupMessage?.GetInvocationList() ?? [])
            {
                var h = (GroupEventHandler)handler;
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
