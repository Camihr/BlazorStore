using AdaStore.Shared.Data;
using AdaStore.Shared.DTOs;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdaStore.Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext context;
        protected readonly IConfiguration configuration;
        protected readonly UserManager<User> userManager;

        public BaseController(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        protected AuthResponse GenerateTokenAuth(UserClaims user)
        {
            var claims = new List<Claim>()
            {
                new Claim("Email", user.Email),
                new Claim("Name", user.Name),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtKey")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            return new AuthResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }  
        
        protected async Task<User> GetUser()
        {
            var email = User.Claims.Where(c => c.Type == "Email").FirstOrDefault();
            return await userManager.FindByEmailAsync(email.Value);
        }
    }
}
