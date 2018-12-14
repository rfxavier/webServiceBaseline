using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendUpdateOrderProductRequest : ServiceRequestBase
    {
        public int OrderId { get; set; }
        public List<OrderProduct> OrderProductList { get; set; }
        public class OrderProduct
        {
            public int ProductID { get; set; }
            public int Quantity { get; set; }
        }
    }
}