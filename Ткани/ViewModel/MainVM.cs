using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ткани.Models;
using Ткани.Tools;
using Ткани.View;

namespace Ткани.ViewModel
{
    public class MainVM : BaseVM
    {
        private User user = new User();
        private Visibility userVisibility = Visibility.Collapsed;
        private Page currentPage;

        public CustomCommand Logout { get; set; }

        public Page CurrentPage 
        { 
            get => currentPage;
            set 
            { 
                currentPage = value; 
                SignalChanged();
            }
        }

        public Visibility UserVisibility
        {
            get => userVisibility;
            set
            {
                userVisibility = value;
                SignalChanged();
            }
        }

        public User User
        {
            get => user;
            set
            {
                UserVisibility = Visibility.Visible;
                user = value;
                SignalChanged();
                SignalChanged("UserName");
                SignalChanged("Role");
            }
        }


        public string Role
        {
            get => user.UserRoleNavigation?.RoleName;
        }


        public string UserName
        {
            get => $"{user.UserName} {user.UserSurname} {user.UserPatronymic}";
        }

        public MainVM()
        {
            CurrentPage = new AuthPage(this);

            Logout = new CustomCommand(() =>
            {
                User = new User();
                UserVisibility = Visibility.Collapsed;
                CurrentPage = new AuthPage(this);
            });
        }
    }
}
