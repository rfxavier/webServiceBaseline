using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendNewSellerOrderRequest : ServiceRequestBase
    {
        public int CustomerID { get; set; }
        public string DeliveryDate { get; set; }
        public int PayMode { get; set; }
        public string PaymentDate { get; set; }
        public bool Sample { get; set; }
        public List<Product> ProductList { get; set; }
        public class Product
        {
            public int Type { get; set; }
            public int ProductId { get; set; }
            public int PeriodType { get; set; }
            public int Quantity { get; set; }
            public int? PriceListId { get; set; }
            public int Discount { get; set; }
        }
    }
}