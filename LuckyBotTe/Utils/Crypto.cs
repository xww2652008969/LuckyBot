using System.Security.Cryptography;
using System.Text;

namespace LuckyBotTe.Utils;

public static class Crypto
{
    public static string ToSHa256(this string data)
    {
        var inputBytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = SHA256.HashData(inputBytes);
        var hashString = new StringBuilder();
        foreach (var b in hashBytes) hashString.Append(b.ToString("x2")); // 转换为小写十六进制
        return hashString.ToString();
    }

    public static string ToSHa256(this byte[] data)
    {
        var hashBytes = SHA256.HashData(data);
        var hashString = new StringBuilder();
        foreach (var b in hashBytes) hashString.Append(b.ToString("x2")); // 转换为小写十六进制
        return hashString.ToString();
    }

    public static long ToSHa256ToLOng(this string data)
    {
        var inputBytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = SHA256.HashData(inputBytes);
        return BitConverter.ToInt64(hashBytes);
    }
}
