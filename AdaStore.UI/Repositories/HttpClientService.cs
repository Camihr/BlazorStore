using System.Text.Json;
using System.Text;
using AdaStore.UI.Interfaces;

namespace AdaStore.UI.Repositories
{
    public class HttpClientService : IHttpClientService
    {
        private string _apiToken;

        public HttpClientService(IConfiguration configuration)
        {
            _apiToken = configuration.GetValue<string>("ApiToken");
        }

        public async Task<HttpResponseMessage> Post<T>(string url, T enviar)
        {
            using (var client = new HttpClient())
            {
                AddBasicAuthHeader(client);
                var enviarJSON = JsonSerializer.Serialize(enviar);
                var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
                return await client.PostAsync(url, enviarContent);
            }
        }

        public async Task<HttpResponseMessage> Put<T>(string url, T body)
        {
            using (var client = new HttpClient())
            {
                AddBasicAuthHeader(client);
                var sendJson = JsonSerializer.Serialize(body);
                var sendContent = new StringContent(sendJson, Encoding.UTF8, "application/json");
                return await client.PutAsync(url, sendContent);
            }
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            using (var client = new HttpClient())
            {
                AddBasicAuthHeader(client);
                return await client.GetAsync(url);
            }
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            using (var client = new HttpClient())
            {
                AddBasicAuthHeader(client);
                return await client.DeleteAsync(url);
            }
        }

        private void AddBasicAuthHeader(HttpClient client)
        {
            byte[] bytes = Encoding.UTF8.GetBytes($"{_apiToken}:ADMIN");
            string base64 = Convert.ToBase64String(bytes);

            var headers = new Dictionary<string, string>()
            {
              { "Authorization", $"Basic {base64}"}
            };

            client.DefaultRequestHeaders.Add(headers.FirstOrDefault().Key, headers.FirstOrDefault().Value);
        }
    }
}