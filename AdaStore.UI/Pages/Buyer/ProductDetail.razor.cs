using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace AdaStore.UI.Pages.Buyer
{
    public partial class ProductDetail
    {
        [Parameter] public Product Product { get; set; }
        [Parameter] public EventCallback<int> AddToCart { get; set; }

        private int _quantity = 1;
        private string _stock;
        private string _errorMessage;

        protected override void OnInitialized()
        {
            if (Product.Stock == 1)
            {
                _stock = "1 unidad";
            }
            else if (Product.Stock > 1) 
            {
                _stock = $"{Product.Stock} unidades";
            }
        }

        private void ModifyQuantity(bool isIncrement)
        {
            if (isIncrement)
                _quantity++;
            else
                _quantity--;

            if (_quantity < 1)
            {
                _errorMessage = "La cantidad no puede ser menor a 1";
            }
            else if (_quantity > Product.Stock)
            {
                var complement = Product.Stock == 1 ? $"añadirá 1 unidad" : $"añadirán {Product.Stock} unidades";
                _errorMessage = $"Superaste la cantidad disponible. Si agregas el producto al carrito, solo se {complement}";
            }
            else
            {
                _errorMessage = string.Empty;
            }
        }

        private async void Add()
        {
            if (_quantity < 1) 
                return;
            else if (_quantity > Product.Stock)
                _quantity = Product.Stock;

            await AddToCart.InvokeAsync(_quantity);
        }
    }
}
