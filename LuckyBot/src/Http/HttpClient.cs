using System.Net.Http.Headers;
using System.Text;
using LuckyBot.Models;
using Newtonsoft.Json;

namespace LuckyBot.Http;

public class HttpApiClient(BotConfig config)
{
    private readonly HttpClient HttpClient = new();
    private string ApiUrl = config.HttpUrl;

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        var response = await HttpClient.GetAsync($"{ApiUrl}/{endpoint}");
        return response;
    }

    public async Task<HttpResult?> PostAsync(string path, string data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, config.HttpUrl + path);

        if (config.HttpToken != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.HttpToken);
        }

        request.Content = new StringContent(data, Encoding.UTF8, "application/json");

        try
        {
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // 确保是成功状态码

            var json = await response.Content.ReadAsStringAsync();
            var httpResult = JsonConvert.DeserializeObject<HttpResult>(json);
            return httpResult;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
