using AdaStore.Shared.DTOs;
using AdaStore.Shared.Models;
using AdaStore.UI.Interfaces;
using System.Text.Json;

namespace AdaStore.UI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IHttpClientService httpClientService;
        private string _apiUrl;
        private JsonSerializerOptions jsonDefaulOptions =>
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public UsersRepository(IConfiguration configuration, IHttpClientService httpClientService, Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClientService = httpClientService;
            _apiUrl = configuration.GetValue<string>("ApiUrl");
        }

        public async Task<HttpResponseBase<AuthResponse>> RegisterUser(UserRegister user)
        {
            try
            {
                var url = $"{_apiUrl}Account/Register";
                var httpResponse = await httpClientService.Post(url, user);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await DeserializeResponse<AuthResponse>(httpResponse, jsonDefaulOptions);

                    return new HttpResponseBase<AuthResponse>()
                    {
                        IsSuccess = httpResponse.IsSuccessStatusCode,
                        Response = httpResponse,
                        Data = response
                    };
                }

                return new HttpResponseBase<AuthResponse>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<AuthResponse>() { IsSuccess = false};
            }
        }

        public async Task<HttpResponseBase<AuthResponse>> Login(LoginCredentials user)
        {
            try
            {
                var url = $"{_apiUrl}Account/Login";
                var httpResponse = await httpClientService.Post(url, user);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await DeserializeResponse<AuthResponse>(httpResponse, jsonDefaulOptions);

                    return new HttpResponseBase<AuthResponse>()
                    {
                        IsSuccess = httpResponse.IsSuccessStatusCode,
                        Response = httpResponse,
                        Data = response
                    };
                }

                return new HttpResponseBase<AuthResponse>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse,                    
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<AuthResponse>() { IsSuccess = false };
            }
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
        }
    }
}
