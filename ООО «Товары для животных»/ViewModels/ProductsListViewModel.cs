using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ООО__Товары_для_животных_.Classes;
using ООО__Товары_для_животных_.Database;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.Models;
using ООО__Товары_для_животных_.Tools;
using ООО__Товары_для_животных_.Views.Pages;

namespace ООО__Товары_для_животных_.ViewModels;

internal class ProductsListViewModel : ViewModel
{
    private MainViewModel mainViewModel;

    private List<Manufacturer> _manufacturers;
    private string _selectedCostSortType;
    private Manufacturer _manufacturer;
    private string _searchText = string.Empty;
    private ObservableCollection<Product> _products;
    private string _rowsCount = string.Empty;
    private Product _selectedProduct;
    IDBSearchProduct iDBSearchProduct;

    public ProductsListViewModel(MainViewModel mainViewModel, IDBListManufacturers iDBListManufacturers, IDBListProducts iDBListProducts, IDBSearchProduct iDBSearchProduct)
    {
        this.mainViewModel = mainViewModel;

        try
        {
            Manufacturers = iDBListManufacturers.GetListManufacturers();
        }
        catch
        {
            MessageBox.Show("Ошибка связи с базой данных. Обратитесь к администратору");
        }

        Manufacturers.Insert(0, new()
        {
            Id = 0,
            Title = "Все производители"
        });

        SelectedManufacturer = Manufacturers[0];

        SelectedCostSortType = CostSortTypes[0];

        SearchForProducts(iDBSearchProduct);

        RemoveProductCommand = new(() =>
        {
            if (SelectedProduct is null)
            {
                MessageBox.Show("Необходимо выбрать товар из списка для его удаления");
                return;
            }
            if (SelectedProduct.OrderProducts.Count > 0)
            {
                MessageBox.Show("Выбранный товар нельзя удалить, поскольку он участвует в заказах");
                return;
            }
            try
            {
                iDBListProducts.GetListProduct().Remove(SelectedProduct);
                DB.Instance.SaveChanges();
                Products.Remove(SelectedProduct);
                MessageBox.Show("Выбранный товар удален");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при удалении товара");
            }
        });

        AddProductCommand = new(() =>
        {
            mainViewModel.CurrentPage = new EditProductPage(new Product(), mainViewModel);
        });
        
        EditProductCommand = new(() =>
        {
            if (SelectedProduct is null)
            {
                MessageBox.Show("Для редактирования продукта нужно его выбрать");
                return;
            }

            mainViewModel.CurrentPage = new EditProductPage(SelectedProduct, mainViewModel);
        });
    }

    public Command RemoveProductCommand { get; set; }
    public Command AddProductCommand { get; set; }
    public Command EditProductCommand { get; set; }

    public Visibility AdminPanelVisibility => Auth.IsUserAdmin ? Visibility.Visible
                                                               : Visibility.Collapsed;

    public string RowsCount
    {
        get => _rowsCount;
        set
        {
            _rowsCount = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Product> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged();
        }
    }

    public Product SelectedProduct
    { 
        get => _selectedProduct;
        set
        {
            _selectedProduct = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            SearchForProducts(iDBSearchProduct);
        }
    }

    public Manufacturer SelectedManufacturer
    {
        get => _manufacturer;
        set
        {
            _manufacturer = value;
            OnPropertyChanged();
            SearchForProducts(iDBSearchProduct);
        }
    }

    public List<Manufacturer> Manufacturers
    {
        get => _manufacturers;
        set
        {
            _manufacturers = value;
            OnPropertyChanged();
        }
    }

    public string[] CostSortTypes { get; set; } =
    {
        "Нет", "По возрастанию", "По Убыванию"
    };

    public string SelectedCostSortType
    {
        get => _selectedCostSortType;
        set
        {
            _selectedCostSortType = value;
            OnPropertyChanged();
            SearchForProducts(iDBSearchProduct);
        }
    }

    private void SearchForProducts(IDBSearchProduct iDBSearchProduct)
    {
        try
        {
            var countAll = DB.Instance.Products.Count();

            var productsQuery = iDBSearchProduct.GetAllIncludeListProduct(SearchText);

            if (SelectedManufacturer.Id != 0)
                productsQuery = productsQuery.Where(s => s.ProductManufacturerId == SelectedManufacturer.Id).ToList();

            productsQuery = SelectedCostSortType switch
            {
                "По возрастанию" => productsQuery.OrderBy(s => s.ProductCost).ToList(),
                "По Убыванию" => productsQuery.OrderByDescending(s => s.ProductCost).ToList(),
                _ => productsQuery,

            };

            var products = productsQuery.ToList();

            products.ForEach(s =>
            {
                s.Image = new BitmapImage();

                s.Image.BeginInit();
                s.Image.CacheOption = BitmapCacheOption.OnLoad;

                if (s.ProductPhoto != null)
                {
                    var ms = new MemoryStream(s.ProductPhoto);
                    s.Image.StreamSource = ms;
                }
                else
                {
                    s.Image.StreamSource = File.OpenRead("Images/picture.png");
                }

                s.Image.EndInit();
                s.Image.StreamSource.Close();
            });

            if (products.Count == 0)
                MessageBox.Show("По данному запросу ничего не найдено");

            RowsCount = $"Выбрано {products.Count} из {countAll} записей";

            Products = new(products);
        }
        catch
        {
            MessageBox.Show("Ошибка в базе данных. Обратитесь к администратору");
        }
    }
}