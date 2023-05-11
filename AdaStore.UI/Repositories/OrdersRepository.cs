using AdaStore.Shared.Models;
using AdaStore.UI.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdaStore.UI.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private JsonSerializerOptions jsonDefaulOptions =>
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        private readonly IHttpClientService httpClientService;
        private string _apiUrl;

        public OrdersRepository(IConfiguration configuration, IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
            _apiUrl = configuration.GetValue<string>("ApiUrl");
        }

        public async Task<HttpResponseBase<object>> AddToCart(CartItem item, int userId)
        {
            try
            {
                var url = $"{_apiUrl}Order/AddToCart?userId={userId}";
                var httpResponse = await httpClientService.Post(url, item);

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

        public async Task<HttpResponseBase<int>> GetQuantityCartItem(int userId)
        {
            try {
                var url = $"{_apiUrl}Order/QuantityCartItem?userId={userId}";
                var httpResponse = await httpClientService.Get(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await DeserializeResponse<int>(httpResponse, jsonDefaulOptions);

                    return new HttpResponseBase<int>()
                    {
                        IsSuccess = httpResponse.IsSuccessStatusCode,
                        Response = httpResponse,
                        Data = response
                    };
                }

                return new HttpResponseBase<int>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse,
                };
            }
            catch (Exception) {
                return new HttpResponseBase<int>() { IsSuccess = false };
            }
        }

        public async Task<HttpResponseBase<Order>> GetCurrentOrder(int userId)
        {
            try
            {
                var url = $"{_apiUrl}Order?userId={userId}";
                var httpResponse = await httpClientService.Get(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await DeserializeResponse<Order>(httpResponse, jsonDefaulOptions);

                    return new HttpResponseBase<Order>()
                    {
                        IsSuccess = httpResponse.IsSuccessStatusCode,
                        Response = httpResponse,
                        Data = response
                    };
                }

                return new HttpResponseBase<Order>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse,
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<Order>() { IsSuccess = false };
            }
        }

        public async Task<HttpResponseBase<List<Order>>> GetCompletedOrders()
        {
            try
            {
                var url = $"{_apiUrl}Order/CompletedOrders";
                var httpResponse = await httpClientService.Get(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await DeserializeResponse<List<Order>>(httpResponse, jsonDefaulOptions);

                    return new HttpResponseBase<List<Order>>()
                    {
                        IsSuccess = httpResponse.IsSuccessStatusCode,
                        Response = httpResponse,
                        Data = response
                    };
                }

                return new HttpResponseBase<List<Order>>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse,
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<List<Order>>() { IsSuccess = false };
            }
        }

        public async Task<HttpResponseBase<object>> RemoveCartItem(int cartItemId)
        {
            try
            {
                var url = $"{_apiUrl}Order/RemoveCartItem?cartItemId={cartItemId}";
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

        public async Task<HttpResponseBase<object>> CancelOrder(int orderId)
        {
            try
            {
                var url = $"{_apiUrl}Order/CancelOrder?orderId={orderId}";
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

        public async Task<HttpResponseBase<object>> Buy(int orderId)
        {
            try
            {
                var url = $"{_apiUrl}Order/Buy?orderId={orderId}";
                var httpResponse = await httpClientService.Get(url);

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
