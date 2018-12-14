using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveMyOrderListResponse : ServiceResponseBase
    {
        public List<MyOrder> MyOrderList { get; set; }
        public class MyOrder
        {
            public int MyOrderID { get; set; }
            public string DateCreated { get; set; }
            public int MyOrderType { get; set; }
            public int Status { get; set; }
            public string ReceiptCode { get; set; }
            public string LocationName { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal ShippingCost { get; set; }
            public List<Product> ProductList { get; set; }
            public class Product
            {
                public int ProductID { get; set; }
                public string ProductName { get; set; }
                public int Quantity { get; set; }
                public decimal Amount { get; set; }
            }
            
        }
    }
}