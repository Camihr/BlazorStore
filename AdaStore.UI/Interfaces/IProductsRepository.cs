using AdaStore.Shared.Models;
using AdaStore.UI.Repositories;

namespace AdaStore.UI.Interfaces
{
    public interface IProductsRepository
    {
        Task<HttpResponseBase<object>> CreateProduct(Product product);
        Task<HttpResponseBase<object>> EditProduct(Product product);
        Task<HttpResponseBase<object>> DeleteProduct(int productId);
        Task<HttpResponseBase<List<Product>>> GetProducts();
    }
}
