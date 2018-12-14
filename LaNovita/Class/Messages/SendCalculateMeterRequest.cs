using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCalculateMeterRequest : ServiceRequestBase
    {
        public int MeterId { get; set; }
        public int QuantityOfMen { get; set; }
        public int QuantityOfWomen { get; set; }
        public int QuantityOfChildren { get; set; }
        public List<MeterCategory> MeterCategoryList { get; set; }

        public class MeterCategory
        {
            public int MeterCategoryId { get; set; }
            public List<MeterCategoryProduct> CategoryProductList { get; set; }
        }

        public class MeterCategoryProduct
        {
            public int ProductId { get; set; }
        }
    }
}