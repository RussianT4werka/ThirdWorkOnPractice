using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ткани.Models;
using Ткани.Tools;
using Ткани.View;

namespace Ткани.ViewModel
{
    internal class ProductVM
    {
        private User user;
        private MainVM mainVM;
        public CustomCommand AddProduct { get; set; }
        public Visibility IsAdminVisibility { get => user.UserRole == 1 ? Visibility.Visible : Visibility.Collapsed; }

        public ProductVM(MainVM mainVM)
        {
            this.user = Auth.Auth.CurrentUser;
            this.mainVM = mainVM;

            AddProduct = new CustomCommand(() =>
            {
                mainVM.CurrentPage = new EditProduct(new Product(), mainVM);
            });
        }
    }
}
