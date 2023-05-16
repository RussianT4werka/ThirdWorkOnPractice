using System;
using System.Collections.Generic;

namespace Ткани.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public int OrderPickupPoint { get; set; }
        public int? UserId { get; set; }
        public int OrderCode { get; set; }
        public string OrderStatus { get; set; } = null!;

        public virtual OrderPickPoint OrderPickupPointNavigation { get; set; } = null!;
        public virtual User? User { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
