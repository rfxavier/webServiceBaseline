using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCategoryListRequest : ServiceRequestBase
    {
        public int SupplierId { get; set; }
        public int CustomerTypeId { get; set; }
        public int CategoryId { get; set; }
        public int CategoryType { get; set; }
    }
}