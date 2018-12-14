using System;
using System.Collections.Generic;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveProductListCatalogResponse : ServiceResponseBase
    {
        public Supplier[] SupplierList;

        public class Product
        {
            public string CategoryName;
            public int ProductId;
            public string ProductName;
            public decimal Price;
            public string PriceStr;
            public decimal OldPrice;
            public string OldPriceStr;
            public DateTime DateCreated;
            public string DateCreatedString;
            public int ImageId;
            public int ThumbImageId;
            public string Manufacturer = " ManufacturerTest";
            public string UnitMeasure = "Kg";
            public int PriceListId;
            public int MinimumQuantity;
            public int MaximumDiscount;
            public bool IsOffer;
            public bool IsNew;
            public int SupplierID;
        }

        public class Seller
        {
            public int SellerID;
            public string Name;
            public int SupplierID;
        }

        public class Supplier
        {
            public int SupplierID { get; set; }
            public string Name { get; set; }
            public string CNPJ;
            public string PhoneNumber;
            public Seller[] SellerList;

            public List<Product> ProductList;

        }
    }
}
