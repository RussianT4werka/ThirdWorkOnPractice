using System;
using System.Collections.Generic;

namespace Ткани.Models
{
    public partial class ProductProvider
    {
        public ProductProvider()
        {
            Products = new HashSet<Product>();
        }

        public int ProviderId { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
