using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveMeterCategoryListResponse : ServiceResponseBase
    {
        public int MeterId { get; set; }
        public IEnumerable<MeterCategory> MeterCategoryList { get; set; }

        public class MeterCategory
        {
            public int MeterCategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryDescription { get; set; }
            public bool Required { get; set; }
            public int Sort { get; set; }
            public IEnumerable<MeterCategoryProduct> CategoryProductList { get; set; }
        }

        public class MeterCategoryProduct
        {
            public int ProductId { get; set; }
            public int PriceListId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public decimal OldPrice { get; set; }
            public int ImageId { get; set; }
            public int ThumbImageId { get; set; }
            public string Manufacturer { get; set; }
            public string UnitMeasure { get; set; }
            public int MinimumQuantity { get; set; }
            public int MaximumDiscount { get; set; }

        }
    }
}