using System.Windows.Controls;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.Models;
using ООО__Товары_для_животных_.ViewModels;

namespace ООО__Товары_для_животных_.Views.Pages;

public partial class EditProductPage : Page
{
    public EditProductPage(Product product, MainViewModel mainViewModel, IDBListManufacturers iDBListManufacturers, IDBListProducts iDBListProducts, IDBSearchProduct iDBSearchProduct)
    {
        InitializeComponent();

        DataContext = new EditProductViewModel(product, mainViewModel, iDBListManufacturers, iDBListProducts, iDBSearchProduct);
    }
}