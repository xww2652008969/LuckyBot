using LuckyBot.Plugins;
using LuckyBot.PostApi;
using LuckyBotTe.Manager;
using LuckyBotTe.Utils;

namespace LuckyBotTe.Plu;

public class Love : Plugin
{
    public override string GetPluginName() => "土味情话";

    private static HttpHelper Httphelp = new();


    public override void Init()
    {
        ActiveSend();
    }

    public override void UnInit() { }

    private static async Task<string> Getlove()
    {
        var response = await Httphelp.GetAsync("https://api.zxki.cn/api/twqh");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return string.Empty;
    }

    public void ActiveSend() => QuartzNetUtil.ScheduleTask(ActiveSendTask, "0 0 * * * ?");

    private void ActiveSendTask()
    {
        Miao();
        Biantaimufei();
    }


    private async Task Miao()
    {
        var s = await Getlove();
        if (s != string.Empty)
        {
            var api = GetPostApi();
            api.CreateGroupMessage(628483968);
            api.ChatMessage.AddAt(827370816).AddText("喵喵你知道吗").AddText(s);
            api.SendGroupMsg();
        }
    }

    private async Task Biantaimufei()
    {
        var s = await Getlove();
        if (s != string.Empty)
        {
            var api = GetPostApi();
            api.CreatePrivateMessage(428975904);
            api.ChatMessage.AddText("宝宝你知道嘛").AddText(s);
            api.SendPrivateMsg();
        }
    }
}
