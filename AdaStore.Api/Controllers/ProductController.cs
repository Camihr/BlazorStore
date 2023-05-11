using AdaStore.Shared.Data;
using AdaStore.Shared.Enums;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace AdaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : BaseController
    {
        public ProductController(ApplicationDbContext context, IConfiguration configuration, UserManager<User> userManager) : base(context, configuration, userManager)
        { }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var productos = await context.Products.ToListAsync();
            return Ok(productos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var user = await GetUser();
            if (user.Profile != Profiles.Admin)
                return Forbid("No tienes autorización para esta operación");

            product.UpdatedAt = DateTime.UtcNow;
            product.CreatedAt = DateTime.UtcNow;

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] Product product)
        {
            var user = await GetUser();
            if (user.Profile != Profiles.Admin)
                return Forbid("No tienes autorización para esta operación");

            var existingProduct = await context.Products.FindAsync(product.Id);

            if (existingProduct == null)
                return BadRequest("El producto solicitado no existe");

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Stock = product.Stock;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeeteProduct([FromQuery] int productId)
        {
            var user = await GetUser();
            if (user.Profile != Profiles.Admin)
                return Forbid("No tienes autorización para esta operación");

            var product = await context.Products.FindAsync(productId);

            if (product == null)
                return BadRequest("El producto solicitado no existe");

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
