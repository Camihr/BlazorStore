using AdaStore.UI.UI;

namespace AdaStore.UI.Shared
{
    public partial class AuthLayout
    {
        private bool _showLoading;
        private bool _showAlert;
        private AlertInfo _alertInfo;
        public void ToogleLoader(bool showLoading)
        {
            _showLoading = showLoading;
            StateHasChanged();
        }

        public void ShowAlert(AlertInfo info)
        {
            _alertInfo = info;
            _showAlert = true;
            StateHasChanged();
        }
    }
}
