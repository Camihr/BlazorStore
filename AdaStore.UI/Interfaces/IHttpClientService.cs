namespace AdaStore.UI.Interfaces
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> Delete(string url);
        Task<HttpResponseMessage> Get(string url);
        Task<HttpResponseMessage> Post<T>(string url, T enviar);
        Task<HttpResponseMessage> Put<T>(string url, T body);
    }
}
