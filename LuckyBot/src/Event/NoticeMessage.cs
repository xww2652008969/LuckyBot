using LuckyBot.Models;

namespace LuckyBot.Event;

public static class NoticeMessage
{
    public delegate void NoticeMessageHandler(Message message);

    public static event NoticeMessageHandler? OnNoticeMessage;

    public static void SendMessage(Message message)
    {
        if (message is { PostType: "notice" })
        {
            foreach (var handler in OnNoticeMessage?.GetInvocationList() ?? [])
            {
                var h = (NoticeMessageHandler)handler;
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
