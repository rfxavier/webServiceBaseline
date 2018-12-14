using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCreateJapoUserOrderRequest : ServiceRequestBase
    {
        public int BuyerId { get; set; }
        public int LocationId { get; set; }
        public int PaymentMethod { get; set; }
        public decimal ChangeOf { get; set; }
        public decimal ShippingCost { get; set; }
        public string DeliveryDate { get; set; }
        public List<OrderProduct> OrderProductList { get; set; }
        public class OrderProduct
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public int? PriceListId { get; set; }
            public int Discount { get; set; }
        }
    }
}