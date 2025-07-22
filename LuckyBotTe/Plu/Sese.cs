using System.Text.RegularExpressions;
using HtmlAgilityPack;
using LuckyBot.Event;
using LuckyBot.Helper;
using LuckyBot.Models;
using LuckyBot.Plugins;
using LuckyBot.PostApi;
using LuckyBotTe.Manager;
using LuckyBotTe.Utils;
using Newtonsoft.Json;
using static System.Text.RegularExpressions.Regex;

namespace LuckyBotTe.Plu;

public class Sese : Plugin
{
    public override string GetPluginName() => "瑟瑟";

    private const string ApiUrl = "http://8591ck.cc/";
    private HttpHelper HttpHelper = new();

    private List<string> M38Ulist = [];

    private Task Inittask;

    public override void Init()
    {
        GroupEvent.OnGroupMessage += Se;
        PrivateEvent.OnPrivateMessage += Se;
        MessageSend.OnMessageSend += Se;
        Inittask = Task.Run(initdata);
    }

    private void Se(Message message)
    {
        if (message.PostType == "message_sent")
        {
            foreach (var m in message.MessageContent)
            {
                if (m.Type == "file")
                {
                    Task.Delay(20_000).ContinueWith(task =>
                    {
                        message.CreateGroupMessage().DeleteMsg(message.MessageId);
                    });
                }
            }
        }


        if (!initdata().IsCompleted) return;
        if (!message.RawMessage.Contains("瑟瑟")) return;
        HttpPostApi api;
        api = message.MessageType == "group" ? message.CreateGroupMessage() : message.CreatePrivateMessage();
        var random = new Random();
        var randomIndex = random.Next(0, M38Ulist.Count);

        var getvidolist = Getvidolist(M38Ulist[randomIndex]).Result;
        randomIndex = random.Next(0, getvidolist.Count);
        var getm38U = Getm38U(ApiUrl + getvidolist[randomIndex].Link).Result;
        Console.WriteLine(getm38U);
        api.ChatMessage.AddFile(getm38U, "好看的文件嘻嘻嘻.m38u");
        if (api.ChatMessage.GroupId != 0)
        {
            api.SendGroupMsg();
            return;
        }

        api.SendPrivateMsg();
    }

    private async Task initdata()
    {
        if (M38Ulist.Count > 0)
        {
            M38Ulist.Clear();
        }

        var directory = GetDirectory();
        foreach (var list in directory.Result)
        {
            M38Ulist.Add(list);
        }
    }

    public override void UnInit()
    {
        GroupEvent.OnGroupMessage -= Se;
        PrivateEvent.OnPrivateMessage -= Se;
        MessageSend.OnMessageSend -= Se;
    }


    private async Task<List<(string Link, string Title)>> Getvidolist(string ur)
    {
        List<(string Link, string Title)> results = [];
        var r = await HttpHelper.GetAsync(ur);
        if (r.IsSuccessStatusCode)
        {
            var t = await r.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(t);
            var liNodes = doc.DocumentNode.SelectNodes("//li");
            if (liNodes == null) return results;
            foreach (var li in liNodes)
            {
                // 提取链接（优先选择标题区域的 <a> 标签）
                var titleLinkNode = li.SelectSingleNode(".//h4[@class='title']/a");
                var link = titleLinkNode?.GetAttributeValue("href", null);
                // 提取标题（优先使用 title 属性，若不存在则用文本）
                var title = titleLinkNode?.GetAttributeValue("title", null);
                if (string.IsNullOrEmpty(title))
                {
                    title = titleLinkNode?.InnerText.Trim(); // 备用：直接取标签文本
                }

                if (!string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(title) && !link.Contains("http"))
                {
                    results.Add((link, title));
                }
            }
        }

        return results;
    }

    private async Task<List<string>> GetDirectory()
    {
        List<string> directorylist = [];
        var httpResponseMessage = await HttpHelper.GetAsync(ApiUrl);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var html = await httpResponseMessage.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var liNodes = doc.DocumentNode.SelectNodes("//ul[@class='stui-pannel__menu clearfix']/li");
            if (liNodes != null)
            {
                foreach (var li in liNodes)
                {
                    // 提取 <a> 标签
                    var aNode = li.SelectSingleNode("a");
                    if (aNode == null) continue;
                    var href = aNode.Attributes["href"].Value;
                    var id = ExtractDigits(href);
                    // 提取 <a> 标签的完整文本（包含分类名称和数字）
                    var aText = aNode.InnerText.Trim();
                    var nu = ExtractNumberAndRest(aText);
                    for (var i = 1; i <= (nu.Number / 40) + 1; i++)
                    {
                        directorylist.Add($"{ApiUrl}/vodtype/{id}-{i}.html");
                    }
                }
            }
        }

        return directorylist;
    }

    private async Task<string> Getm38U(string url)
    {
        var task = HttpHelper.GetAsync(url).Result;
        if (!task.IsSuccessStatusCode) return string.Empty;
        var pattern = @"var player_aaaa\s*=\s*\{([\s\S]*?)\}";
        var match = Regex.Match(task.Content.ReadAsStringAsync().Result, pattern, RegexOptions.Singleline);
        if (match.Success)
        {
            var jsonLikeString = match.Groups[0].Value;
            var innerPattern = @"var player_aaaa\s*=\s*\{\s*([\s\S]*?)\s*\}";
            var innerMatch = Regex.Match(jsonLikeString, innerPattern);
            if (!innerMatch.Success || innerMatch.Groups.Count <= 1) return string.Empty;
            var withBraces = $"{{{innerMatch.Groups[1].Value}}}";
            var player = JsonConvert.DeserializeObject<PlayerAaaa>(withBraces);
            return player.Url;
        }
        else
        {
            return string.Empty;
        }
    }

    //提取数字
    private static string ExtractDigits(string input) => Replace(input, "[^0-9]", "");


    //分割
    private static (int Number, string rest) ExtractNumberAndRest(string input)
    {
        var pattern = @"^(\d+\.?\d*)(.*)$";
        var match = Match(input, pattern);

        if (match.Success)
        {
            int.TryParse(match.Groups[1].Value, out int number);
            var rest = match.Groups[2].Value;
            return (number, rest);
        }
        else
        {
            return (0, input);
        }
    }

    private struct PlayerAaaa
    {
        [JsonProperty("flag")]
        public string Flag { get; set; }

        [JsonProperty("encrypt")]
        public int Encrypt { get; set; }

        [JsonProperty("trysee")]
        public int Trysee { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("link_next")]
        public string LinkNext { get; set; }

        [JsonProperty("link_pre")]
        public string LinkPre { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("url_next")]
        public string UrlNext { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } // 注意：示例中是字符串（带引号）

        [JsonProperty("sid")]
        public int Sid { get; set; }

        [JsonProperty("nid")]
        public int Nid { get; set; }
    }
}
