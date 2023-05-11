using AdaStore.Shared.Models;
using AdaStore.UI.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using AdaStore.Shared.Enums;
using Microsoft.AspNetCore.Routing;

namespace AdaStore.UI.Shared
{
    public partial class MainLayout
    {
        [Inject] protected NavigationManager Navigation { get; set; }
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        public User CurrentUser { get; set; }
        public bool IsMenuClosed { get; set; }

        private string _title;
        private bool _showLoading;
        private bool _showAlert;
        private int _quantityItems;
        private AlertInfo _alertInfo;

        protected async override Task OnInitializedAsync()
        {
            var user = (await AuthStat).User;

            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                CurrentUser = new User();

                if (Enum.TryParse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value, out Profiles role))
                {
                    if (role != Profiles.Admin && role != Profiles.Buyer)
                    {
                        Navigation.NavigateTo("/login");
                        return;
                    }

                    CurrentUser.Profile = role;
                }

                CurrentUser.Email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                CurrentUser.Name = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                var uri = Navigation.Uri;
                var absoluteUri = Navigation.ToAbsoluteUri(uri);

                var buyerRoutes = new List<string>() { "/register", "/products", "/cart" };
                var adminRoutes = new List<string>() { "/register", "/admin/products", "/admin/transactions", "/admin/users" };

                switch (CurrentUser.Profile)
                {
                    case Profiles.Admin:
                        if (!adminRoutes.Any(r => r == absoluteUri.AbsolutePath))
                            Navigation.NavigateTo("/admin/transactions");
                        break;
                    case Profiles.Buyer:
                        if (!buyerRoutes.Any(r => r == absoluteUri.AbsolutePath))
                            Navigation.NavigateTo("/products");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Navigation.NavigateTo("/register");
            }
        }

        public void SetTitle(string title)
        {
            _title = title;
            StateHasChanged();
        }

        public void ToogleLoader(bool showLoading)
        {
            _showLoading = showLoading;
            StateHasChanged();
        }

        public void ToogleMenu(bool isMinuClosed)
        {
            IsMenuClosed = isMinuClosed;
            StateHasChanged();
        }

        public void ShowAlert(string message, bool isError)
        {
            _alertInfo = new AlertInfo()
            {
                Message = message,
                IsError = isError
            };

            _showAlert = true;
            StateHasChanged();
        }

        public void SetQuantityItems(int quantity, QuantityOperations operation)
        {

            switch (operation)
            {
                case QuantityOperations.Set:
                    _quantityItems = quantity;
                    break;
                case QuantityOperations.Add:
                    _quantityItems += quantity;
                    break;
                case QuantityOperations.Subtract:
                    _quantityItems -= quantity;
                    break;
                default:
                    break;
            }

            StateHasChanged();
        }
    }
}
