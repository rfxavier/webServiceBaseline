using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveSellerCustomerListRequest: ServiceRequestBase
    {
        public string Query { get; set; }
        public int Page { get; set; }
    }
}