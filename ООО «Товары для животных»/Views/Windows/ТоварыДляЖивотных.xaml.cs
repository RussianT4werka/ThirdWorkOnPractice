using System.Windows;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.ViewModels;

namespace ООО__Товары_для_животных_.Views.Windows;

public partial class ТоварыДляЖивотных : Window
{
    public ТоварыДляЖивотных(IDBListManufacturers iDBListManufacturers, IDBListProducts iDBListProducts, IDBSearchProduct iDBSearchProduct)
    {
        InitializeComponent();

        DataContext = new MainViewModel(iDBListManufacturers, iDBListProducts, iDBSearchProduct);
    }
}