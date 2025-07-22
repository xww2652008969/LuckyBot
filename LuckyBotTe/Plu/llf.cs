using LuckyBot.Event;
using LuckyBot.Helper;
using LuckyBot.Models;
using LuckyBot.Plugins;
using LuckyBotTe.Manager;
using LuckyBotTe.Utils;

namespace LuckyBotTe.Plu;

public class Llf : Plugin
{
    private ConfigManager.PluginPath pluginPath;
    private static List<string> imgpath = [];
    private HttpHelper HttpHelper = new();
    private static bool IsCach = true;

    public override string GetPluginName() => "母肥图片";

    public override void Init()
    {
        pluginPath = ConfigManager.CreatePluginPath("Llf");
        InitData();
        GroupEvent.OnGroupMessage += SendImg;
        PrivateEvent.OnPrivateMessage += P;
        PrivateEvent.OnPrivateMessage += Cach;
    }

    private void InitData()
    {
        if (imgpath.Count > 0)
        {
            imgpath.Clear();
        }

        var files = Directory.GetFiles(pluginPath.DataPath);
        foreach (var file in files)
        {
            Console.WriteLine(file);
            imgpath.Add(file);
        }
    }

    private void SendImg(Message message)
    {
        if (!message.RawMessage.Contains("母肥")) return;
        var random = new Random();
        var i = random.Next(1, 3);
        var postApi = message.CreateGroupMessage();
        for (var j = 0; j < i; j++)
        {
            var ii = random.Next(0, imgpath.Count);
            postApi.ChatMessage.AddImage(imgpath[ii]);
        }

        postApi.SendGroupMsg();
    }

    private void P(Message message)
    {
        if (!message.RawMessage.Contains("母肥")) return;
        var random = new Random();
        var i = random.Next(1, 3);
        var postApi = message.CreatePrivateMessage();
        for (var j = 0; j < i; j++)
        {
            var ii = random.Next(0, imgpath.Count);
            postApi.ChatMessage.AddImage(imgpath[ii]);
        }

        postApi.SendPrivateMsg();
    }

//储存
    private void Cach(Message message)
    {
        if (message.UserId != 1271701079) return;


        if (!IsCach) return;

        var flag = false;
        foreach (var m in message.MessageContent)
        {
            if (m.Type == "image")
            {
                var url = m.Data.Url;
                Downimg(url);
                flag = true;
            }

            if (m.Type != "forward") continue;
            {
                var postApi = GetPostApi();
                var result = postApi.GetForwardMsg(m.Data.Id).Result;
                foreach (var rumlist in result.Data.Messages.SelectMany(rum => rum.MessageContent))
                {
                    switch (rumlist.Type)
                    {
                        case "image":
                        {
                            var url = rumlist.Data.Url;
                            Downimg(url);
                            flag = true;
                            break;
                        }
                        case "forward":
                        {
                            var urllist = new List<string?>();
                            foreach (var l in rumlist.Data.Content.Select(rumdata => rumdata.MessageContent
                                                                              .Where(s => s.Type == "image")
                                                                              .Select(u => u.Data.Url)
                                                                              .ToList()).Where(l => l.Count > 0))
                            {
                                urllist.AddRange(l);
                            }

                            Downimg(urllist.ToArray());
                            flag = true;
                            break;
                        }
                    }
                }
            }
        }

        if (!flag) return;
        InitData();
        var api = message.CreatePrivateMessage();
        api.ChatMessage.AddText("储存成功");
        api.SendPrivateMsg();
    }

    private void Downimg(params string?[] urls)
    {
        foreach (var url in urls)
        {
            if (string.IsNullOrEmpty(url))
                continue;

            var result = HttpHelper.GetAsync(url).Result;
            if (result.IsSuccessStatusCode)
            {
                var bytes = result.Content.ReadAsByteArrayAsync().Result;
                if (bytes.Length > 0)
                {
                    var sHa256 = Crypto.ToSHa256(bytes);
                    if (!File.Exists(pluginPath.DataPath + sHa256 + ".jpg"))
                    {
                        pluginPath.WriteDataBytes(sHa256 + ".jpg", bytes);
                    }
                }
            }
        }
    }


    public override void UnInit()
    {
        GroupEvent.OnGroupMessage -= SendImg;
        PrivateEvent.OnPrivateMessage -= P;
        PrivateEvent.OnPrivateMessage -= Cach;
    }
}
