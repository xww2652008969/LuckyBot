using LuckyBot.Http;
using LuckyBot.Models;
using Newtonsoft.Json;

namespace LuckyBot.PostApi;

public class HttpPostApi(BotConfig config)
{
    public ChatMessage? ChatMessage;
    private readonly HttpApiClient HttpApiClient = new(config);

    public HttpPostApi CreateGroupMessage(long groupId)
    {
        ChatMessage = new ChatMessage(groupId, 0);
        return this;
    }

    public HttpPostApi CreatePrivateMessage(long userId)
    {
        ChatMessage = new ChatMessage(0, userId);
        return this;
    }

    public HttpPostApi CreateMessage(long groupId, long userId)
    {
        ChatMessage = new ChatMessage(groupId, userId);
        return this;
    }

    public Task<HttpResult?> SendGroupMsg()
    {
        var jsonString = JsonConvert.SerializeObject(ChatMessage, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        Console.WriteLine(jsonString);
        return HttpApiClient.PostAsync("/send_group_msg", jsonString);
    }

    public Task<HttpResult?> SendPrivateMsg()
    {
        var jsonString = JsonConvert.SerializeObject(ChatMessage, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        Console.WriteLine(jsonString);
        return HttpApiClient.PostAsync("/send_private_msg", jsonString);
    }

    public Task<HttpResult?> SendForwardMsg(String data) => HttpApiClient.PostAsync("/send_forward_msg", data);

    public async Task<HttpResult?> SendGroupPoke(long groupId, long userId)
    {
        var dict = new Dictionary<string, string>
        {
            { "group_id", groupId.ToString() },
            { "user_id", userId.ToString() }
        };
        var jsonString = JsonConvert.SerializeObject(dict);
        return await HttpApiClient.PostAsync("/group_poke", jsonString);
    }

    public async Task<HttpResult?> SendPrivatePokeAsync(long userId)
    {
        var dict = new Dictionary<string, string>
        {
            { "user_id", userId.ToString() }
        };
        var jsonString = JsonConvert.SerializeObject(dict);
        return await HttpApiClient.PostAsync("/friend_poke", jsonString);
    }

    public async Task<HttpResult?> DeleteMsg(long msgId)
    {
        var dict = new Dictionary<string, string>
        {
            { "message_id", msgId.ToString() }
        };
        var jsonString = JsonConvert.SerializeObject(dict);
        return await HttpApiClient.PostAsync("/delete_msg", jsonString);
    }

    public async Task<HttpResult?> GetGroupMemberList(long groupId)
    {
        var dict = new Dictionary<string, string>
        {
            { "group_id", groupId.ToString() }
        };
        var jsonString = JsonConvert.SerializeObject(dict);
        return await HttpApiClient.PostAsync("/get_group_member_list", jsonString);
    }

    public async Task<HttpResult?> GetGroupMemberInfo(long groupId, long userId)
    {
        var dict = new Dictionary<string, string>
        {
            { "group_id", groupId.ToString() },
            { "user_id", userId.ToString() }
        };
        var jsonString = JsonConvert.SerializeObject(dict);
        return await HttpApiClient.PostAsync("/get_group_member_info", jsonString);
    }

    public async Task<HttpResult?> GetForwardMsg(string messageId)
    {
        var d = new Dictionary<string, string>
        {
            { "message_id", messageId.ToString() }
        };
        var jsonString = JsonConvert.SerializeObject(d);
        return await HttpApiClient.PostAsync("/get_forward_msg", jsonString);
    }
}
