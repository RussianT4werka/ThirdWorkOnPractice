using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Ткани.DB;
using Ткани.Models;
using Ткани.Tools;
using Ткани.View;

namespace Ткани.ViewModel
{
    public class AuthVM : BaseVM
    {
        private MainVM mainVM;
        private Visibility capchaVisible = Visibility.Collapsed;
        private readonly Canvas capchaCanvas;
        private bool canEnter = true;
        public string Login { get; set; }
        public string CapchaText { get; set; }
        string capchaValue;
        public CustomCommand LoginUser { get; set; }
        public CustomCommand LoginGuest { get; set; }

        public bool CanEnter
        {
            get => canEnter;
            set
            {
                canEnter = value;
                SignalChanged();
            }
        }

        public Visibility CapchaVisible
        {
            get => capchaVisible;
            set
            {
                capchaVisible = value;
                SignalChanged();
            }
        }

        public AuthVM(MainVM mainVM, Canvas capchaCanvas, PasswordBox textPassword)
        {
            this.mainVM = mainVM;
            this.capchaCanvas = capchaCanvas;

            LoginUser = new CustomCommand(() =>
            {
                string pass = textPassword.Password;
                if(CapchaVisible == Visibility.Collapsed)
                {
                    CheckLoginAndPassword(pass);
                }
                else
                {
                    if(capchaValue == CapchaText)
                    {
                        CheckLoginAndPassword(pass);
                    }
                    else
                    {
                        MessageBox.Show("Введённый текст не совпадает с капчей");
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(10);
                        timer.Tick += Timer_Tick;
                        timer.Start();

                        CanEnter = false;
                        GenerateCapcha();
                    }
                }
            });

            LoginGuest = new CustomCommand(() =>
            {
                mainVM.User = new User { UserRoleNavigation = new Role { RoleName = "Гость" } };
                mainVM.CurrentPage = new ProductPage(mainVM);
            });

        }

        private void GenerateCapcha()
        {
            capchaCanvas.Children.Clear();
            Random rnd = new Random();
            string value = "";
            string temp;
            for (int i = 0; i < 4; i++)
            {
                if (rnd.NextDouble() > 0.3)
                {
                    temp = ((char)rnd.Next(65, 91)).ToString();
                    if (rnd.NextDouble() > 0.5)
                        temp = temp.ToLower();
                }
                else
                    temp = ((char)rnd.Next(48, 58)).ToString();
                TextBlock text = new TextBlock();
                text.Text = temp;
                text.FontSize = 35;
                capchaCanvas.Children.Add(text);
                Canvas.SetLeft(text, i * 10);
                Canvas.SetTop(text, 5 + rnd.Next(-5, 5));
                value += temp;
            }
            capchaValue = value;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            CanEnter = true;
        }

        private void CheckLoginAndPassword(string pass)
        {
            User user = null;
            try
            {
                user = user04Context.GetInstance().Users.Include("UserRoleNavigation").FirstOrDefault(s => s.UserPassword == pass && s.UserLogin == Login);
            }
            catch
            {
                MessageBox.Show("Ошибка связи с БД");
            }
            if (user == null)
            {
                MessageBox.Show("Данный пользователь не существует");
                CapchaVisible = Visibility.Visible;
                GenerateCapcha();
            }
            else
                EnterUser(user);
        }

        private void EnterUser(User user)
        {
            Auth.Auth.CurrentUser = user;
            mainVM.User = user;
            mainVM.CurrentPage = new ProductPage(mainVM);
        }
    }
}
