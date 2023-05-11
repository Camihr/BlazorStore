using AdaStore.Shared.Conts;
using AdaStore.Shared.Models;
using AdaStore.UI.Interfaces;
using AdaStore.UI.Repositories;
using AdaStore.UI.Shared;
using AdaStore.UI.UI;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace AdaStore.UI.Pages.Admin
{
    public partial class AdminProducts
    {
        [Inject] public IProductsRepository ProductsRepository { get; set; }
        [CascadingParameter] public MainLayout Layout { get; set; }

        private List<Product> _products;
        private List<Product> _allProducts;
        private List<TableColumn> _columns;
        private string _searchText;
        private string _emptyMessage;
        private bool _isEditing;
        private bool _isManagementVisible;
        private Product _selectedProduct;

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Layout.ToogleLoader(true);
                Layout.SetTitle("Productos");
                SetColumns();
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
                _products = _allProducts;
                Order();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private async Task CreateProduct()
        {
            var response = await ProductsRepository.CreateProduct(_selectedProduct);

            if (response.IsSuccess)
            {
                Layout.ShowAlert("Producto creado de manera exitosa", false);
                await GetProducts();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private async Task EditProduct()
        {
            var response = await ProductsRepository.EditProduct(_selectedProduct);

            if (response.IsSuccess)
            {
                Layout.ShowAlert("Producto editado de manera exitosa", false);
                await GetProducts();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }
        }

        private async Task DeleteProduct(Product product)
        {
            Layout.ToogleLoader(true);
            var response = await ProductsRepository.DeleteProduct(product.Id);

            if (response.IsSuccess)
            {
                Layout.ShowAlert("Producto eliminado de manera exitosa", false);
                await GetProducts();
            }
            else
            {
                Layout.ShowAlert(await response.GetErrorMessage(), true);
            }

            Layout.ToogleLoader(false);
        }

        private async void SaveProduct()
        {
            Layout.ToogleLoader(true);
            _isManagementVisible = false;
            if (_isEditing)
            {
                await EditProduct();
            }
            else
            {
                await CreateProduct();  
            }
            Layout.ToogleLoader(false);
        }

        private void OpenManagementProduct(Product product = null)
        {
            if (product == null)
            {
                _isEditing = false;
                _selectedProduct = new Product();
            }
            else
            {
                _isEditing = true;
                _selectedProduct = product;
            }

            _isManagementVisible = true;
        }

        private void SetColumns()
        {
            _columns = new List<TableColumn>()
            {
                new TableColumn(){DisplayName = "Imagen", NotOrder = true},
                new TableColumn(){PropName = "Name", DisplayName = "Nombre", IsSelected = true, IsDesc = false},
                new TableColumn(){PropName = "Stock", DisplayName = "Stock"},
                new TableColumn(){PropName = "Price", DisplayName = "Precio"},
                new TableColumn(){DisplayName = "Editar", OptionalStyles ="th-center", NotOrder = true},
                new TableColumn(){DisplayName = "Borrar", OptionalStyles ="th-center", NotOrder = true},
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
            if (_allProducts != null)
            {
                if (string.IsNullOrEmpty(_searchText))
                {
                    _products = _allProducts;
                }
                else
                {
                    _products = _allProducts
                            .Where(c => c.Name.ToUpper().Contains(_searchText.ToUpper()))
                            .ToList();
                }
            }

            if (_products == null || !_products.Any())
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
            PropertyInfo prop = typeof(Product).GetProperty(selectedColumn.PropName);

            if (selectedColumn.IsDesc)
                _products = _products.OrderByDescending(x => prop.GetValue(x, null)).ToList();
            else
                _products = _products.OrderBy(x => prop.GetValue(x, null)).ToList();
        }
    }
}
