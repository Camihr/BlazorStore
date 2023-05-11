using Microsoft.AspNetCore.Components;

namespace AdaStore.UI.Shared
{
    public partial class LoginDisplay
    {
        [CascadingParameter] MainLayout Layout { get; set; }

        private bool _showOptions;

        private void OpenOptions()
        {

        }
    }
}
