using Microsoft.AspNetCore.Components;

namespace AdaStore.UI.Shared
{
    public partial class HeaderBar
    {
        [CascadingParameter] MainLayout Layout { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public int QuantityItems { get; set; }

        private bool _isMenuClosed;

        private void ToogleSideBar()
        {
            _isMenuClosed = !_isMenuClosed;
            Layout.ToogleMenu(_isMenuClosed);
        }
    }
}
