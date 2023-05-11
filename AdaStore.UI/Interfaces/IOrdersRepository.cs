using AdaStore.Shared.Models;
using AdaStore.UI.Repositories;

namespace AdaStore.UI.Interfaces
{
    public interface IOrdersRepository
    {
        Task<HttpResponseBase<object>> AddToCart(CartItem item, int userId);
        Task<HttpResponseBase<object>> Buy(int orderId);
        Task<HttpResponseBase<object>> CancelOrder(int orderId);
        Task<HttpResponseBase<List<Order>>> GetCompletedOrders();
        Task<HttpResponseBase<Order>> GetCurrentOrder(int userId);
        Task<HttpResponseBase<int>> GetQuantityCartItem(int userId);
        Task<HttpResponseBase<object>> RemoveCartItem(int cartItemId);
    }
}
