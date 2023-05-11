using AdaStore.Shared.DTOs;
using AdaStore.UI.Interfaces;
using AdaStore.UI.Shared;
using AdaStore.UI.UI;
using Microsoft.AspNetCore.Components;

namespace AdaStore.UI.Pages
{
    public partial class Login
    {
        [Inject] IUsersRepository UsersRepository { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [CascadingParameter] public AuthLayout Layout { get; set; }

        private LoginCredentials user = new LoginCredentials();

        private async Task LoginUser()
        {
            Layout.ToogleLoader(true);

            var response = await UsersRepository.Login(user);

            if (response.IsSuccess)
            {
                Navigation.NavigateTo("/login", true);
            }
            else
            {
                Layout.ShowAlert(new AlertInfo() { IsError = true, Message = await response.GetErrorMessage() });
            }

            Layout.ToogleLoader(false);
        }
    }
}
