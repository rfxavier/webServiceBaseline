using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCalculateMeterResponse : ServiceResponseBase
    {
        public int MeterId { get; set; }
        public List<MeterCategory> MeterCategoryList { get; set; }

        public class MeterCategory
        {
            public int MeterCategoryId { get; set; }
            public int Sort { get; set; }
            public List<MeterCategoryProduct> CategoryProductList { get; set; }
        }

        public class MeterCategoryProduct
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}