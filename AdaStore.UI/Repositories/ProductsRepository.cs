using AdaStore.Shared.Models;
using AdaStore.UI.Interfaces;
using System.Text.Json;

namespace AdaStore.UI.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private JsonSerializerOptions jsonDefaulOptions =>
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        private readonly IHttpClientService httpClientService;
        private string _apiUrl;

        public ProductsRepository(IConfiguration configuration, IHttpClientService httpClientService)
        {
            _apiUrl = configuration.GetValue<string>("ApiUrl");
            this.httpClientService = httpClientService;
        }

        public async Task<HttpResponseBase<List<Product>>> GetProducts()
        {
            try
            {
                var url = $"{_apiUrl}Product";
                var httpResponse = await httpClientService.Get(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await DeserializeResponse<List<Product>>(httpResponse, jsonDefaulOptions);

                    return new HttpResponseBase<List<Product>>()
                    {
                        IsSuccess = httpResponse.IsSuccessStatusCode,
                        Response = httpResponse,
                        Data = response
                    };
                }

                return new HttpResponseBase<List<Product>>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse,
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<List<Product>>() { IsSuccess = false };
            }
        }

        public async Task<HttpResponseBase<object>> CreateProduct(Product product)
        {
            try
            {
                var url = $"{_apiUrl}Product";
                var httpResponse = await httpClientService.Post(url, product);

                return new HttpResponseBase<object>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<object>() { IsSuccess = false };
            }
        }

        public async Task<HttpResponseBase<object>> EditProduct(Product product)
        {
            try
            {
                var url = $"{_apiUrl}Product";
                var httpResponse = await httpClientService.Put(url, product);

                return new HttpResponseBase<object>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<object>() { IsSuccess = false };
            }
        }

        public async Task<HttpResponseBase<object>> DeleteProduct(int productId)
        {
            try
            {
                var url = $"{_apiUrl}Product?productId={productId}";
                var httpResponse = await httpClientService.Delete(url);

                return new HttpResponseBase<object>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<object>() { IsSuccess = false };
            }
        }


        private async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
        }
    }
}
