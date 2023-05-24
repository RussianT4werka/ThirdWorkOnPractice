using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ООО__Товары_для_животных_.Classes;
using ООО__Товары_для_животных_.Database;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.Models;
using ООО__Товары_для_животных_.Tools;
using ООО__Товары_для_животных_.Views.Pages;

namespace ООО__Товары_для_животных_.ViewModels;

public class LoginViewModel : ViewModel
{
    private MainViewModel mainViewModel;
    private Visibility capchaVisible = Visibility.Collapsed;

    private DispatcherTimer _timer;

    public string Login { get; set; }
    public string CapchaText { get; set; }

    public Command LoginCommand { get; set; }
    public Command LoginAsGuestCommand { get; set; }

    public bool CanEnter
    {
        get => canEnter;
        set
        {
            canEnter = value;
            OnPropertyChanged();
        }
    }

    public Visibility CapchaVisible
    {
        get => capchaVisible;
        set
        {
            capchaVisible = value;
            OnPropertyChanged();
        }
    }

    public LoginViewModel(MainViewModel mainViewModel, Canvas capchaCanvas, PasswordBox textPassword, IDBListManufacturers iDBListManufacturers, IDBListProducts iDBListProducts, IDBSearchProduct iDBSearchProduct)
    {
        this.mainViewModel = mainViewModel;

        _timer = new()
        {
            Interval = TimeSpan.FromSeconds(10)
        };

        _timer.Tick += Timer_Tick;

        LoginCommand = new Command(() =>
        {
            string pass = textPassword.Password;

            if (CapchaVisible != Visibility.Collapsed)
            {
                if (capchaValue != CapchaText)
                {
                    MessageBox.Show("Введенные символы не совпадают с капчей. Попробуйте ещё раз");

                    _timer.Start();

                    CanEnter = false;

                    capchaValue = Capcha.GenerateOnCanvas(capchaCanvas);

                    return;
                }
            }

            var user = Auth.ValidateUser(pass, Login, new AuthDBProvider());

            if (user is not null)
            {
                LoginAsUser(user, iDBListManufacturers, iDBListProducts, iDBSearchProduct);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите символы с картинки");
                CapchaVisible = Visibility.Visible;

                capchaValue = Capcha.GenerateOnCanvas(capchaCanvas);
            }
        }, () => CanEnter);

        LoginAsGuestCommand = new Command(() =>
        {
            mainViewModel.User = new User 
            { 
                UserRoleNavigation = new Role 
                { 
                    RoleName = "Гость" 
                }
            };

            mainViewModel.CurrentPage = new ProductsListPage(mainViewModel, iDBListManufacturers, iDBListProducts, iDBSearchProduct);
        });
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timer.Stop();
        CanEnter = true;
    }

    string capchaValue;
    private bool canEnter = true;

    private void LoginAsUser(User user, IDBListManufacturers iDBListManufacturers, IDBListProducts iDBListProducts, IDBSearchProduct iDBSearchProduct)
    {
        mainViewModel.User = user;

        mainViewModel.CurrentPage = new ProductsListPage(mainViewModel, iDBListManufacturers, iDBListProducts, iDBSearchProduct);
    }
}