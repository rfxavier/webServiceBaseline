using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendNewMyOrderRequest
    {
        public string Identification;
        public string SerialKey;

        public NewOrder[] NewOrderList;

        public class NewOrder
        {
            public int SupplierID { get; set; }
            public int SellerID { get; set; }
            public Product[] ProductList;
        }

        public class Product
        {
            public int ProductID;
            public int Quantity;
            public decimal Amount;
        }
    }
}