using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCreateMyOrderRequest : ServiceRequestBase
    {
        public List<OrderProduct> OrderProductList { get; set; }
        public int LocationID { get; set; }
        public int PaymentMethod { get; set; }
        public decimal ChangeOf { get; set; }
        public decimal ShippingCost { get; set; }
        public string DeliveryDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Base64StringImage { get; set; }
        public string Reference { get; set; }

        public class OrderProduct
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public int? PriceListId { get; set; }
            public int Discount { get; set; }

        }
    }
}