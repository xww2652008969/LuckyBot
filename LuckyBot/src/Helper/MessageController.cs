using LuckyBot.Models;
using LuckyBot.PostApi;

namespace LuckyBot.Helper;

public static class MessageHelper
{
    public static HttpPostApi? CreateGroupMessage(this Message message)
    {
        if (message.GroupId == 0) return null;
        var config = message.GetBotConfig();
        return config == null ? null : new HttpPostApi(config.Value).CreateGroupMessage(message.GroupId);
    }

    public static HttpPostApi? CreatePrivateMessage(this Message message)
    {
        if (message.UserId == 0) return null;
        var config = message.GetBotConfig();
        return config == null ? null : new HttpPostApi(config.Value).CreatePrivateMessage(message.UserId);
    }

    public static HttpPostApi? CreateMessage(this Message message, long groupId, long userId)
    {
        var config = message.GetBotConfig();
        return config == null ? null : new HttpPostApi(config.Value).CreateMessage(groupId, userId);
    }
}
