using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveOrderListResponse : ServiceResponseBase
    {
        public List<Order> OrderList;

        public class Order
        {
            public int OrderID { get; set; }
            public string Address { get; set; }
            public string OrderNumber { get; set; }
            public string Client { get; set; }
            public string DateOrder { get; set; }
            public int Status { get; set; }
            public string OrderType { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string SellerName { get; set; }

        }
    }
}