using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveOrderResponse : ServiceResponseBase
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public string Client { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string Address { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string DateOrder { get; set; }
        public int StatusId { get; set; }
        public string SellerName { get; set; }
        public string SellerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public string PayMethod { get; set; }
        public int ImageId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Product> ProductList { get; set; }
        public class Product
        {
            public int ProductID { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public string UnitMeasure { get; set; }
            public string Manufacturer { get; set; }
        }
    }
}