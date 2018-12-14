using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCustomerListRequest : ServiceRequestBase
    {
        public string Query { get; set; }
        public bool JapoUser { get; set; }
    }
}