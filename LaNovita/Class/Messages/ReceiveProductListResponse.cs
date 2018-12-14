using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveProductListResponse : ServiceResponseBase
    {
        public List<Product> ProductList;

        public class Product
        {
            public string CategoryName;
            public int ProductId;
            public string ProductName;
            public decimal Price;
            public decimal OldPrice;
            public DateTime DateCreated;
            public int ImageId;
            public int ThumbImageId;
            public string Manufacturer = " ManufacturerTest";
            public string UnitMeasure = "Kg";
            public int PriceListId;
            public int MinimumQuantity;
            public int MaximumDiscount;
        }
    }
}