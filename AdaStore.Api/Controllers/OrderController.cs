using AdaStore.Shared.Data;
using AdaStore.Shared.Enums;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AdaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public OrderController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart([FromQuery] int userId, [FromBody] CartItem item)
        {
            var order = await context.Orders
                .Include(o => o.CartItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatuses.InProcess);

            var product = await context.Products.FindAsync(item.ProductId);
            product.Stock -= item.Quantity;
            product.UpdatedAt = DateTime.UtcNow;

            try
            {
                if (order == null)
                {
                    order = new Order()
                    {
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        UserId = userId,
                        CartItems = new List<CartItem> { item }
                    };

                    context.Orders.Add(order);
                }
                else
                {
                    var existingCartItem = order.CartItems.FirstOrDefault(c => c.ProductId == item.ProductId);

                    if (existingCartItem == null)
                    {
                        item.OrderId = order.Id;
                        context.CartItems.Add(item);
                    }
                    else
                    {
                        existingCartItem.Quantity += item.Quantity;
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok();
        }

        [HttpGet]
        [Route("QuantityCartItem")]
        public async Task<IActionResult> GetQuantityCartItem([FromQuery] int userId)
        {
            var order = await context.Orders
               .Include(o => o.CartItems)
               .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatuses.InProcess);

            var quantity = 0;

            if (order != null && order.CartItems.Any())
                quantity = order.CartItems.Sum(o => o.Quantity);

            return Ok(quantity);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentOrder([FromQuery] int userId)
        {
            var order = await context.Orders
               .Include(o => o.CartItems)
               .ThenInclude(o => o.Product)
               .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatuses.InProcess);

            if (order == null)
                return BadRequest("La orden solicitada no existe");

            foreach (var item in order.CartItems)
                item.Order = null;

            return Ok(order);
        }

        [HttpDelete]
        [Route("RemoveCartItem")]
        public async Task<IActionResult> RemoveCartItem([FromQuery] int cartItemId)
        {
            var cartItem = await context.CartItems
                .Include(c=>c.Product)
                .FirstOrDefaultAsync(c=>c.Id == cartItemId);

            if (cartItem == null)
                return BadRequest("El item solicitado no existe");

            var order = await context.Orders
                .Include(o => o.CartItems)
                .FirstOrDefaultAsync(o => o.Id == cartItem.OrderId);

            if (order.CartItems.Count <=1)
            {
                order.Status = OrderStatuses.Canceled;
                order.IsDeleted = true;
                order.UpdatedAt = DateTime.UtcNow;
            }

            cartItem.Product.Stock += cartItem.Quantity;
            cartItem.Product.UpdatedAt = DateTime.UtcNow;

            context.CartItems.Remove(cartItem);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromQuery] int orderId)
        {
            var order = await context.Orders
                .Include(o=>o.CartItems)
                .ThenInclude(o=>o.Product)
                .FirstOrDefaultAsync(o=>o.Id == orderId);

            if (order == null)
                return BadRequest("La orden solicitada no existe");

            foreach (var item in order.CartItems)
            {
                item.Product.Stock += item.Quantity;
                item.Product.UpdatedAt = DateTime.UtcNow;
            }

            order.Status = OrderStatuses.Canceled;
            order.IsDeleted = true;
            order.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("Buy")]
        public async Task<IActionResult> Buy([FromQuery] int orderId)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("CompletOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter orderIdParameter = new SqlParameter("@OrderId", SqlDbType.Int);
                    orderIdParameter.Value = orderId;
                    command.Parameters.Add(orderIdParameter);

                    SqlParameter newDateParameter = new SqlParameter("@NewDate", SqlDbType.DateTime);
                    newDateParameter.Value = DateTime.UtcNow;
                    command.Parameters.Add(newDateParameter);

                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }
        
        [HttpGet]
        [Route("CompletedOrders")]
        public async Task<IActionResult> GetCompletedOrders()
        {
            var orders = await context.Orders
               .Include(o => o.User)
               .Include(o => o.CartItems)
               .ThenInclude(o => o.Product)
               .IgnoreQueryFilters()
               .Where(o => o.Status == OrderStatuses.Completed)
               .ToArrayAsync();

            foreach (var order in orders)
            {
                foreach (var item in order.CartItems)
                    item.Order = null;
            }

            return Ok(orders);
        }
    }
}