using AdaStore.Shared.Conts;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;

        public AccountController(
            UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> BuyerRegistration([FromBody] User user)
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
                Profile = user.Profile,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var result = await userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Conts.Buyer);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
