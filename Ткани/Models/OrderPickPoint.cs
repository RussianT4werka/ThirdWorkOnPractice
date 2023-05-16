using System;
using System.Collections.Generic;

namespace Ткани.Models
{
    public partial class OrderPickPoint
    {
        public OrderPickPoint()
        {
            Orders = new HashSet<Order>();
        }

        public int PickPointId { get; set; }
        public string Adress { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
