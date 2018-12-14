using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Class
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ImageURL { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int DiscountMax { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
