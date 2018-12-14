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
    public class ReceiveListProductResponse : ServiceResponseBase
    {
        public Product[] ProductList;

        public class Product
        {
            public int ProductId;
            public string ProductName;
            public decimal Price;
            public DateTime DateCreated;
            public int ImageId;
            public int ThumbImageId;
            public string Manufacturer = " ManufacturerTest";
            public string UnitMeasure = "Kg";
        }

    }
}