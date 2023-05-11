using AdaStore.Shared.Data;
using AdaStore.Shared.DTOs;
using AdaStore.Shared.Models;
using AdaStore.UI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdaStore.UI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> dbFactory;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpClientService httpClientService;
        
        private string _apiUrl;

        public UsersRepository(
            IConfiguration configuration, 
            IHttpClientService httpClientService,
            IDbContextFactory<ApplicationDbContext> dbFactory,
            Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider authenticationStateProvider,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.dbFactory = dbFactory;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpClientService = httpClientService;

            _apiUrl = configuration.GetValue<string>("ApiUrl");
        }

        public async Task<HttpResponseBase<AuthResponse>> RegisterUser(UserRegister user)
        {
            try
            {
                var url = $"{_apiUrl}Account/Register";
                var httpResponse = await httpClientService.Post(url, user);

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

                return new HttpResponseBase<AuthResponse>()
                {
                    IsSuccess = httpResponse.IsSuccessStatusCode,
                    Response = httpResponse
                };
            }
            catch (Exception)
            {
                return new HttpResponseBase<AuthResponse>() { IsSuccess = false };
            }
        }
    }
}
