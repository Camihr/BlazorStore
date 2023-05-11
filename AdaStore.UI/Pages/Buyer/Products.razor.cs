using AdaStore.Shared.Conts;
using AdaStore.Shared.Models;
using AdaStore.UI.Shared;
using Microsoft.AspNetCore.Components;
using AdaStore.UI.Interfaces;
using AdaStore.UI.UI;

namespace AdaStore.UI.Pages.Buyer
{
    public partial class Products
    {
        [Inject] protected IProductsRepository ProductsRepository { get; set; }
        [Inject] protected IOrdersRepository OrdersRepository { get; set; }

        [CascadingParameter] public MainLayout Layout { get; set; }

        private List<Product> _products;
        private List<Product> _allProducts;
        private Product _selectedProduct;
        private string _emptyMessage;
        private bool _isDesc;
        private bool _isOrdered;
        private bool _isDetailVisible;

        private string TextOrderButton => _isDesc ? "Mayor precio primero" : "Menor precio primero";

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Layout.ToogleLoader(true);
                Layout.SetTitle("Productos");
                await GetProducts();
                Layout.ToogleLoader(false);
            }
        }

        private async Task GetProducts()
        {           
            var response = await ProductsRepository.GetProducts();

            if (response.IsSuccess)
            {
                _allProducts = response.Data;
                _products = _allProducts.OrderBy(p => p.Name).ToList();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }          
        }


        private async Task AddToCart(int quantity)
        {
            Layout.ToogleLoader(true);

            _isDetailVisible = false;

            var cartItem = new CartItem()
            {
                ProductId = _selectedProduct.Id,
                Quantity = quantity,
                UnitPrice = _selectedProduct.Price
            };

            var response = await OrdersRepository.AddToCart(cartItem, Layout.CurrentUser.Id);

            if (response.IsSuccess)
            {
                _selectedProduct.Stock -= quantity;
                Layout.SetQuantityItems(quantity, QuantityOperations.Add);
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }

            StateHasChanged();
            Layout.ToogleLoader(false);
        }

        private async void SelecteProduct(Product product, bool toDetail)
        {
            _selectedProduct = product;

            if (toDetail)
            {
                _isDetailVisible = true;
            }
            else
            {
                await AddToCart(1);
            }
        }

        private void Search(string searchText)
        {
            if (_allProducts != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    _products = _allProducts;
                }
                else
                {
                    _products = _allProducts
                            .Where(c => c.Name.ToUpper().Contains(searchText.ToUpper()))
                            .ToList();
                }
            }

            if (_products == null || !_products.Any())
            {
                _emptyMessage = Conts.NoSearchResults;
            }
            else if (_isOrdered)
            {
                Order();
            }
        }

        private void Order()
        {
            _isOrdered = true;

            if (_isDesc)
                _products = _products.OrderByDescending(p => p.Price).ToList();
            else
                _products = _products.OrderBy(p => p.Price).ToList();

            _isDesc = !_isDesc;
        }
    }
}
