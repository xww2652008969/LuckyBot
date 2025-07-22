using System.Net;
using System.Text;

namespace LuckyBotTe.Manager
{
    public class HttpHelper
    {
        private readonly HttpClient Client;

        public HttpHelper()
        {
            var handler = new HttpClientHandler();
            handler.Proxy = HttpClient.DefaultProxy;
            handler.UseProxy = true;

            Client = new HttpClient(handler);
        }


        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (headers != null)
            {
                SetHeaders(request, headers);
            }

            return await Client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostJsonAsync(
            string url, Dictionary<string, string>? headers, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            SetHeaders(request, headers);
            return await Client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string url, string json) =>
            await PostJsonAsync(url, null, json);

        private void SetHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
        {
            if (headers is not { Count: > 0 }) return;
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
