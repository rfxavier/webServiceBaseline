using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveProductListRequest : ServiceRequestBase
    {
        public int CategoryId { get; set; }
        public int SupplierID { get; set; }
        public int CustomerTypeId { get; set; }
        public int Page { get; set; }
        public bool Offer { get; set; }
        public bool IsNew { get; set; }

        public ReceiveProductListRequest()
        {
            this.IsNew = false;
        }
    }
}