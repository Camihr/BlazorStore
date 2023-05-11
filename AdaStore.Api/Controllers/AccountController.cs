using AdaStore.Shared.Conts;
using AdaStore.Shared.Data;
using AdaStore.Shared.DTOs;
using AdaStore.Shared.Enums;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace AdaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly SignInManager<User> signInManager;

        public AccountController(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
            : base(context, configuration, userManager)
        {
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> BuyerRegistration([FromBody] UserRegister user)
        {
            if (!ModelState.IsValid)
                return BadRequest(Conts.ServerError);

            var existingUser = await userManager.FindByNameAsync(user.Email.Trim());

            if (existingUser != null)
                return BadRequest("Ya existe un usuario con este correo");

            var newUser = new User()
            {
                Name = user.Name,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.Email,
                Document = user.Document,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Profile = Profiles.Buyer
            };

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Conts.Buyer);
                return Ok(GenerateTokenAuth(new UserClaims() { Name = newUser.Name, Email = newUser.Email }));
            }
            else
            {
                return BadRequest("Ocurrió un error al registrarte, por favor vuélvelo a intentar");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials credential)
        {
            var result = await signInManager.PasswordSignInAsync(
                credential.Email,
                credential.Password,
                isPersistent: credential.IsPersistent,
                lockoutOnFailure: false);

            var user = await GetUser();

            if (result.Succeeded)
            {
                var authResponse = GenerateTokenAuth(new UserClaims() { Name = user.Name, Email = user.Email });
                return Ok(authResponse);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpGet]
        [Route("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RenewToken()
        {
            var email = User.Claims.Where(c => c.Type == "Email").FirstOrDefault();            
            return Ok(GenerateTokenAuth(new UserClaims() { Email = email.Value }));
        }
    }
}
