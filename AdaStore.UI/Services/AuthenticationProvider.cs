using AdaStore.Shared.Conts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AdaStore.UI.Services
{
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim (ClaimTypes.Email, "c4camilo@gmail.com"),
                    new Claim (ClaimTypes.Name, "Camilo"),
                    new Claim(ClaimTypes.Role, Conts.Buyer)
                },
            authenticationType: "test") ;
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        }
    }
}
