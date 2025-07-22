using System.Text.RegularExpressions;
using LuckyBot.Models;
using Microsoft.VisualBasic;
using static System.Text.RegularExpressions.Regex;

namespace LuckyBotTe.Utils;

public static class QqHelp
{
    public static bool IsAt(this Message message, out long qq)
    {
        qq = 0;
        if (message.MessageContent == null) return false;
        foreach (var data in message.MessageContent.Where(data => data.Type == "at"))
        {
            qq = Convert.ToInt64(data.Data.Qq);
            return true;
        }

        return false;
    }

    public static string GetUserName(string? card, string nickname) => card ?? nickname;
}
