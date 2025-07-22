using System.Runtime.InteropServices;
using System.Text;
using LuckyBot.Event;
using LuckyBot.Helper;
using LuckyBot.Models;
using LuckyBot.Plugins;
using LuckyBotTe.Manager;
using LuckyBotTe.Utils;

namespace LuckyBotTe.Plu;

public class Wife : Plugin
{
    private ConfigManager.PluginPath? Config;
    private const string ApiUrl = "https://flat-dove-71.deno.dev/";
    private static readonly List<string> ImgPath = [];
    private static readonly HttpHelper HttpHelp = new();

    private Task? InitTask;


    public override string GetPluginName() => "今日二次元老婆";

    public override void Init()
    {
        Config = ConfigManager.CreatePluginPath("Wife");
        InitTask = Task.Run(Initialize);
        GroupEvent.OnGroupMessage += GroupHanldr;
    }

    private void Initialize()
    {
        if (ImgPath.Count > 0) return;
        if (Config == null) return;
        try
        {
            var response = HttpHelp.GetAsync(ApiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    var filename = line.Trim();
                    var imgPath = Path.Combine(Config.DataPath, filename);

                    if (!File.Exists(imgPath))
                    {
                        Task.Run(() =>
                        {
                            var downloadResponse = HttpHelp.GetAsync($"{ApiUrl}{filename}").Result;
                            if (!downloadResponse.IsSuccessStatusCode) return;
                            var data = downloadResponse.Content.ReadAsByteArrayAsync().Result;
                            var written = Config.WriteDataBytes(filename, data);
                            if (written)
                            {
                                ImgPath.Add(imgPath);
                            }
                        });
                    }
                    else
                    {
                        ImgPath.Add(imgPath);
                    }
                }
            }
            else
            {
                Console.WriteLine("初始化失败");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
    }

    private void GroupHanldr(Message message)
    {
        if (!InitTask.IsCompleted) return;
        if (message.RawMessage != "今日老婆") return;
        var l = Math.Abs($"{message.UserId}{DateTime.UtcNow}".ToSHa256ToLOng() % ImgPath.Count);
        var postApi = message.CreateGroupMessage();
        var chatMessage = postApi?.ChatMessage?.AddReply(message.MessageId).AddImage(ImgPath[(int)l])
                                 .AddText("今日二次元老婆");
        postApi?.SendGroupMsg();
    }

    public override void UnInit() => GroupEvent.OnGroupMessage -= GroupHanldr;
}
