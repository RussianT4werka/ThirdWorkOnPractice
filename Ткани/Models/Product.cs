using System;
using System.Collections.Generic;

namespace Ткани.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public string ProductArticleNumber { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductUnit { get; set; } = null!;
        public decimal ProductCost { get; set; }
        public byte ProductMaxDiscount { get; set; }
        public int ProductManufacturer { get; set; }
        public int ProductProvider { get; set; }
        public int ProductCategory { get; set; }
        public byte? ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductDescription { get; set; } = null!;
        public byte[]? ProductPhoto { get; set; } 
        public string? ProductStatus { get; set; }

        public virtual ProductCategory ProductCategoryNavigation { get; set; } = null!;
        public virtual ProductManufacturer ProductManufacturerNavigation { get; set; } = null!;
        public virtual ProductProvider ProductProviderNavigation { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
