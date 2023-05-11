using AdaStore.Shared.Models;
using AdaStore.UI.Components;
using AdaStore.UI.Interfaces;
using AdaStore.UI.Shared;
using AdaStore.UI.UI;
using Microsoft.AspNetCore.Components;

namespace AdaStore.UI.Pages.Buyer
{
    public partial class Cart
    {
        [Inject] protected IOrdersRepository OrdersRepository { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }

        [CascadingParameter] public MainLayout Layout { get; set; }

        private Order _order;
        private bool _isConfirmVisible;
        private bool _isBuying;

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Layout.ToogleLoader(true);
                Layout.SetTitle("Carrito");
                await GetOrder();
                Layout.ToogleLoader(false);
            }
        }

        private async void Confirm()
        {
            Layout.ToogleLoader(true);

            _isConfirmVisible = false;

            if (_isBuying)
            {
                await Buy();
            }
            else
            {
                await CancelOrder();
            }

            Layout.ToogleLoader(false);
        }

        private void CalculateTotal()
        {
            _order.Total = 0;

            foreach (var item in _order.CartItems)
            {
                item.Total = item.Quantity * item.Product.Price;
                _order.Total += item.Total;
            }

            StateHasChanged();
        }

        private async Task GetOrder()
        {
            var response = await OrdersRepository.GetCurrentOrder(Layout.CurrentUser.Id);

            if (response.IsSuccess)
            {
                _order = response.Data;
                CalculateTotal();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private async Task RemoveCartItem(CartItem item)
        {
            var response = await OrdersRepository.RemoveCartItem(item.Id);

            if (response.IsSuccess)
            {
                Layout.SetQuantityItems(item.Quantity, QuantityOperations.Subtract);
                _order.CartItems.Remove(item);

                CalculateTotal();

                if (!_order.CartItems.Any())
                {
                    Navigation.NavigateTo("/products");
                }
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private async Task CancelOrder()
        {
            var response = await OrdersRepository.CancelOrder(_order.Id);

            if (response.IsSuccess)
            {
                Layout.SetQuantityItems(0, QuantityOperations.Set);
                Navigation.NavigateTo("/products");
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private async Task Buy()
        {
            var response = await OrdersRepository.Buy(_order.Id);

            if (response.IsSuccess)
            {
                Layout.SetQuantityItems(0, QuantityOperations.Set);
                Navigation.NavigateTo("/products");
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }
    }
}
