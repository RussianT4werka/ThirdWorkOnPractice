using System.Windows.Controls;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.ViewModels;

namespace ООО__Товары_для_животных_.Views.Pages;

public partial class ProductsListPage : Page
{
    public ProductsListPage(MainViewModel mainViewModel, IDBListManufacturers iDBListManufacturers, IDBListProducts iDBListProducts, IDBSearchProduct iDBSearchProduct)
    {
        InitializeComponent();

        DataContext = new ProductsListViewModel(mainViewModel, iDBListManufacturers, iDBListProducts, iDBSearchProduct);
    }
}