using AdaStore.Shared.Conts;
using AdaStore.Shared.Models;
using AdaStore.UI.Components;
using AdaStore.UI.Interfaces;
using AdaStore.UI.Repositories;
using AdaStore.UI.Shared;
using AdaStore.UI.UI;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace AdaStore.UI.Pages.Admin
{
    public partial class Transactions
    {
        [Inject] protected IOrdersRepository OrdersRepository { get; set; }
        [CascadingParameter] public MainLayout Layout { get; set; }

        private List<Order> _orders;
        private List<Order> _allOrders;
        private List<TableColumn> _columns;
        private string _searchText;
        private string _emptyMessage;
        private bool _isUserDetailVisible;
        private User _selectedUser;
        private bool _isOrderDetailVisible;
        private Order _selectedOrder;

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Layout.ToogleLoader(true);
                Layout.SetTitle("Transacciones");
                SetColumns();
                await GetCompletedOrders();
                Layout.ToogleLoader(false);
            }
        }

        private async Task GetCompletedOrders()
        {
            var response = await OrdersRepository.GetCompletedOrders();

            if (response.IsSuccess)
            {
                _allOrders = response.Data;
                CalculateTotals();
                _orders = _allOrders;

                Order();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private void ViewUserDetail(User user)
        {
            _selectedUser = user;
            _isUserDetailVisible = true;
        }

        private void ViewOrderDetail(Order order)
        {
            _selectedOrder = order;
            _isOrderDetailVisible = true;
        }

        private void CalculateTotals()
        {
            foreach (var order in _allOrders)
            {
                foreach (var item in order.CartItems)
                {
                    item.Total = item.UnitPrice * item.Quantity;
                    order.Total += item.Total;
                }
            }
        }

        private void SetColumns()
        {
            _columns = new List<TableColumn>()
            {
                new TableColumn(){PropName = "Id", DisplayName = "#"},
                new TableColumn(){PropName = "UpdatedAt", DisplayName = "Fecha", IsSelected = true, IsDesc = true},
                new TableColumn(){PropName = "Total", DisplayName = "Total"},
                new TableColumn(){DisplayName = "Detalle", OptionalStyles ="th-center", NotOrder = true},
                new TableColumn(){PropName = "UserName",DisplayName = "Usuario", OptionalStyles ="th-center", NotOrder = true},
            };
        }

        private void Search(string searchText)
        {
            _searchText = searchText;
            Filter();
        }

        private void SetOrder(TableColumn column)
        {
            var selectedColumn = _columns.FirstOrDefault(c => c.IsSelected);

            if (selectedColumn.PropName == column.PropName)
            {
                column.IsDesc = !column.IsDesc;
            }
            else
            {
                selectedColumn.IsSelected = false;
                column.IsSelected = true;
            }

            Order();
        }

        private void Filter()
        {
            if (_allOrders != null)
            {
                if (string.IsNullOrEmpty(_searchText))
                {
                    _orders = _allOrders;
                }
                else
                {
                    _orders = _allOrders
                            .Where(c => c.UserName.ToUpper().Contains(_searchText.ToUpper())
                            || c.Id.ToString().Contains(_searchText))
                            .ToList();
                }
            }

            if (_orders == null || !_orders.Any())
            {
                _emptyMessage = Conts.NoSearchResults;
            }
            else
            {
                Order();
            }
        }

        private void Order()
        {
            var selectedColumn = _columns.FirstOrDefault(c => c.IsSelected);
            PropertyInfo prop = typeof(Order).GetProperty(selectedColumn.PropName);

            if (selectedColumn.IsDesc)
                _orders = _orders.OrderByDescending(x => prop.GetValue(x, null)).ToList();
            else
                _orders = _orders.OrderBy(x => prop.GetValue(x, null)).ToList();
        }
    }
}
